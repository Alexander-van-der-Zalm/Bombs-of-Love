using UnityEngine;
using System.Collections;
using System;

public class Bomb : MonoBehaviour
{
    //public GameObject Explosion;
    public Explosion ExplosionPrefab;

    public int BaseRange = 3;
    public int BaseDamage = 1;
    public float DetonateTime = 3.0f;

    private Animator anim;
    private int animExplode = Animator.StringToHash("Explode");
    



    public void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Detonate(Grid grid, int extraRange = 0, int extraDamage = 0, float DetonateOverride = -1)
    {
        float detonateTime = DetonateOverride != -1 ? DetonateOverride : DetonateTime;

        StartCoroutine(DetonateCR(grid, BaseRange + extraDamage, BaseDamage + extraDamage, detonateTime));
    }

    private IEnumerator DetonateCR(Grid grid, int range, int damage, float detonateTime)
    {
        // Wait for detonation
        yield return new WaitForSeconds(detonateTime);
        // Set anim
        anim.SetTrigger(animExplode);
        
        // Spawn explosions
        // Center pos
        Vector2 gridPos = grid.GetCurrentGridPos(transform.position);
        Debug.Log("Initial pos: " + gridPos);
        Spawn(ExplosionPrefab, grid, gridPos, damage, Explosion.ExplosionRotation.Center, Explosion.ExplosionType.Center);

        // Spawn in four directions
        for(int i = 1; i < range; i++)
        {
            Explosion.ExplosionType type = i == range - 1 ? Explosion.ExplosionType.End : Explosion.ExplosionType.Mid;
            Spawn(ExplosionPrefab, grid, gridPos + new Vector2(0,i), damage, Explosion.ExplosionRotation.Top, type);
            Spawn(ExplosionPrefab, grid, gridPos + new Vector2(0,-i), damage, Explosion.ExplosionRotation.Bottom, type);
            Spawn(ExplosionPrefab, grid, gridPos + new Vector2(i,0), damage, Explosion.ExplosionRotation.Right, type);
            Spawn(ExplosionPrefab, grid, gridPos + new Vector2(-i,0), damage, Explosion.ExplosionRotation.Left, type);
        }

        // Wait to destroy Self

        //// Destroy Self
        //GameObject.Destroy(this);
    }

    private void Spawn(Explosion explosion, Grid grid, Vector2 gridPos, int damage, Explosion.ExplosionRotation rotation, Explosion.ExplosionType type)
    {
        Debug.Log(gridPos);

        if (gridPos.x < 0 || gridPos.y < 0 || gridPos.x >= grid.GridColumns || gridPos.y >= grid.GridRows)
        {
            Debug.Log("Explosion grid location not legal - out of range ");
            return;
        }
        
        // Check if Legal - Take into account grid types
        GridElement element = grid.GetGridElement((int)gridPos.x, (int)gridPos.y);
        if(element.Type != GridElement.GridType.Floor)
        {
            // Not legal
            Debug.Log("Explosion grid location not legal " + element.Type);
            return;
        }

        // Transform to worldpos
        Vector3 worldPos = grid.GetGridWorldPos((int)gridPos.x, (int)gridPos.y) + new Vector3(Grid.GridWidth/2,0);

        

        // Spawn object
        Spawn(explosion, worldPos, damage, rotation, type);
    }

    private void Spawn(Explosion explosion, Vector3 position, int damage, Explosion.ExplosionRotation rotation, Explosion.ExplosionType type)
    {
        // Spawn Object
        GameObject go = (GameObject.Instantiate(explosion.gameObject, position, Quaternion.identity) as GameObject);
        Explosion newExplode = go.GetComponent<Explosion>();
        newExplode.SetType(type, rotation);
        newExplode.Damage = damage;
        //Debug.Log("spawned pos " + go.transform.position); 
    }
}
