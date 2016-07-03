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

    public void Detonate(int extraRange = 0, int extraDamage = 0, float DetonateOverride = -1)
    {
        float detonateTime = DetonateOverride != 1 ? DetonateOverride : DetonateTime;

        StartCoroutine(DetonateCR(BaseRange + extraDamage, BaseDamage + extraDamage, detonateTime));
    }

    private IEnumerator DetonateCR(int range, int damage, float detonateTime)
    {
        // Wait for detonation
        yield return new WaitForSeconds(detonateTime);

        // Set anim
        anim.SetTrigger(animExplode);

        // Spawn explosions
        Spawn(ExplosionPrefab, transform.position, damage, Explosion.ExplosionRotation.Center, Explosion.ExplosionType.Center);

        // Take into account grid types
        for(int i = 0; i < range; i++)
        {

        }

        // Wait to destroy Self

        //// Destroy Self
        //GameObject.Destroy(this);
    }

    private void Spawn(Explosion explosion, Vector3 position, int damage, Explosion.ExplosionRotation rotation, Explosion.ExplosionType type)
    {
        Explosion newExplode = (GameObject.Instantiate(explosion, position, Quaternion.identity) as GameObject).GetComponent<Explosion>();
        newExplode.SetType(type, rotation);
        newExplode.Damage = damage;
    }
}
