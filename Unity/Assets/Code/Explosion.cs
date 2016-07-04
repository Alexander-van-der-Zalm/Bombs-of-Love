using UnityEngine;
using System.Collections;
using System;

public class Explosion : MonoBehaviour
{
    public enum ExplosionRotation
    {
        Top,Bottom,Left,Right,Center
    }
    public enum ExplosionType
    {
        End,Mid,Center
    }

    public ExplosionRotation Rotation;
    public ExplosionType Type;

    public int Damage;

    private int animRotation = Animator.StringToHash("Rotation");
    private int animType = Animator.StringToHash("Type");

    private Animator anim;

    public void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SetType(ExplosionType type, ExplosionRotation rotation)
    {
        //Debug.Log("I am type: " + type + " rot " + rotation);
        Type = type;
        Rotation = rotation;
        // Set anim
        anim.SetInteger(animRotation,(int)Rotation);
        anim.SetInteger(animType, (int)Type);
    }

    // Trigger Enter
    public void OnTriggerEnter2D(Collider2D other)
    {
        Health health = other.GetComponent<Health>();
        if(health != null)
        {
            health.DoDamge(Damage);
        }
        // Bomb chain
        //Bomb bomb = other.GetComponent<Bomb>();
        //if(bomb != null)
        //{
        //    bomb.Detonate();
        //}
    }
}
