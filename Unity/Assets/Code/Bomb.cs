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
        Debug.Log(Time.time);
        yield return new WaitForSeconds(detonateTime);
        Debug.Log(detonateTime);
        Debug.Log(Time.time);
        // Set anim
        anim.SetTrigger(animExplode);
        
        // Spawn explosions
        // Center pos
        Vector2 gridPos = grid.GetCurrentGridPos(transform.position);
        Spawn(ExplosionPrefab, grid, gridPos, damage, Explosion.ExplosionRotation.Center, Explosion.ExplosionType.Center);

        // Spawn in four directions
        for(int i = 0; i < range; i++)
        {
            Explosion.ExplosionType type = i == range - 1 ? Explosion.ExplosionType.End : Explosion.ExplosionType.Mid;
            Spawn(ExplosionPrefab, grid, gridPos + new Vector2(0,i), damage, Explosion.ExplosionRotation.Right, type);
            Spawn(ExplosionPrefab, grid, gridPos + new Vector2(0,-i), damage, Explosion.ExplosionRotation.Left, type);
            Spawn(ExplosionPrefab, grid, gridPos + new Vector2(i,0), damage, Explosion.ExplosionRotation.Top, type);
            Spawn(ExplosionPrefab, grid, gridPos + new Vector2(-i,0), damage, Explosion.ExplosionRotation.Bottom, type);
        }

        // Wait to destroy Self

        //// Destroy Self
        //GameObject.Destroy(this);
    }

    private void Spawn(Explosion explosion, Grid grid, Vector2 gridPos, int damage, Explosion.ExplosionRotation rotation, Explosion.ExplosionType type)
    {
        // Check if Legal - Take into account grid types
        GridElement element = grid.GetGridElement((int)gridPos.x, (int)gridPos.y);
        if(element.Type != GridElement.GridType.Floor)
        {
            // Not legal
            Debug.Log("Explosion grid location not legal " + element.Type);
            return;
        }

        // Transform to worldpos
        Vector3 worldPos = grid.GetGridWorldPos((int)gridPos.x, (int)gridPos.y);
        
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
    }
}
