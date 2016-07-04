using UnityEngine;
using System.Collections;
using System;

public class Bomb : MonoBehaviour
{
    #region Fields

    public Explosion ExplosionPrefab;

    public int BaseRange = 3;
    public int BaseDamage = 1;
    public float DetonateTime = 3.0f;

    private Animator anim;
    private int animExplode = Animator.StringToHash("Explode");

    private int range = 0;
    private int damage = 0;
    private float detonateTime = 0;
    private Grid grid = null;
    private bool exploded = false;


    #endregion

    public void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Detonate(Grid grid, int extraRange = 0, int extraDamage = 0, float DetonateOverride = -1)
    {
        // Set variables
        range = BaseRange + extraRange;
        damage = BaseDamage + extraDamage;
        detonateTime = DetonateOverride != -1 ? DetonateOverride : DetonateTime;
        this.grid = grid;
        exploded = false;

        // Start Bomb Coroutine
        StartCoroutine(DetonateCR());
    }

    private IEnumerator DetonateCR()
    {
        // Wait for detonation
        yield return new WaitForSeconds(detonateTime);

        if (exploded)
            yield break;

        // Trigger spawning of explosion objects & cleanup
        DetonateNow();
    }

    public void DetonateNow()
    {
        // Set anim
        anim.SetTrigger(animExplode);

        // Spawn explosions
        // Center pos
        Vector2 gridPos = grid.GetCurrentGridPos(transform.position);
        Spawn(ExplosionPrefab, grid, gridPos, Explosion.ExplosionRotation.Center, Explosion.ExplosionType.Center);

        // Spawn in four directions
        bool left = true, right = true, top = true, bottom = true;
        for (int i = 1; i <= range; i++) // If one cannot spawn - stop it from happening
        {
            Explosion.ExplosionType type = i == range ? Explosion.ExplosionType.End : Explosion.ExplosionType.Mid;
            if (top) top = Spawn(ExplosionPrefab, grid, gridPos + new Vector2(0, i), Explosion.ExplosionRotation.Top, type);
            if (bottom) bottom = Spawn(ExplosionPrefab, grid, gridPos + new Vector2(0, -i), Explosion.ExplosionRotation.Bottom, type);
            if (right) right = Spawn(ExplosionPrefab, grid, gridPos + new Vector2(i, 0), Explosion.ExplosionRotation.Right, type);
            if (left) left = Spawn(ExplosionPrefab, grid, gridPos + new Vector2(-i, 0), Explosion.ExplosionRotation.Left, type);
        }

        // Wait to destroy Self
        StopAllCoroutines();

        //// Destroy Self
        GameObject.Destroy(this.gameObject);
    }

    private bool Spawn(Explosion explosion, Grid grid, Vector2 gridPos, Explosion.ExplosionRotation rotation, Explosion.ExplosionType type)
    {
        //Debug.Log(gridPos);

        if (gridPos.x < 0 || gridPos.y < 0 || gridPos.x >= grid.GridColumns || gridPos.y >= grid.GridRows)
        {
            Debug.Log("Explosion grid location not legal - out of range ");
            return false;
        }
        
        // Check if Legal - Take into account grid types
        GridElement element = grid.GetGridElement((int)gridPos.x, (int)gridPos.y);
        if(element.Type != GridElement.GridType.Floor)
        {
            // Not legal
            Debug.Log("Explosion grid location not legal " + element.Type);
            return false;
        }

        // Transform to worldpos
        Vector3 worldPos = grid.GetGridWorldPos((int)gridPos.x, (int)gridPos.y) + new Vector3(Grid.GridWidth/2,0);

        // Spawn object
        Spawn(explosion, worldPos, rotation, type);
        return true;
    }

    private void Spawn(Explosion explosion, Vector3 position, Explosion.ExplosionRotation rotation, Explosion.ExplosionType type)
    {
        // Spawn Object
        GameObject go = (GameObject.Instantiate(explosion.gameObject, position, Quaternion.identity) as GameObject);
        Explosion newExplode = go.GetComponent<Explosion>();
        newExplode.SetType(type, rotation);
        newExplode.Damage = damage;
    }
}
