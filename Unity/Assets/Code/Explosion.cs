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



    public void SetType(ExplosionType type, ExplosionRotation rotation)
    {
        Debug.Log("I am type: " + type);
        Type = type;
        Rotation = rotation;
        // Set anim
    }

    // Trigger Enter
}
