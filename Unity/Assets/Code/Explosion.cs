using UnityEngine;
using System.Collections;
using System;

public class Explosion : MonoBehaviour
{
    #region Enums

    public enum ExplosionRotation
    {
        Top, Bottom, Left, Right, Center
    }
    public enum ExplosionType
    {
        End, Mid, Center
    }

    #endregion

    #region Fields

    public ExplosionRotation Rotation;
    public ExplosionType Type;

    public int Damage;

    private Animator anim;
    private int animRotation = Animator.StringToHash("Rotation");
    private int animType = Animator.StringToHash("Type");
    private int animFinishedState = Animator.StringToHash("Finished");
    #endregion

    #region Awake

    public void Awake()
    {
        anim = GetComponent<Animator>();
    }

    #endregion

    #region Start

    public void Initiate(ExplosionType type, ExplosionRotation rotation, int damage)
    {
        //Debug.Log("I am type: " + type + " rot " + rotation);
        Type = type;
        Rotation = rotation;
        Damage = damage;

        // Set anim
        anim.SetInteger(animRotation, (int)Rotation);
        anim.SetInteger(animType, (int)Type);

        // Start CleanupCR
        StartCoroutine(CleanupCR());
    }

    private IEnumerator CleanupCR()
    {
        int steps = 0;

        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);

        // Wait to go out of finished
        while (info.shortNameHash == animFinishedState)
        {
            info = anim.GetCurrentAnimatorStateInfo(0);
            steps++;
            yield return null;
        }

        // Then wait to go back to finished
        while (info.shortNameHash != animFinishedState)
        {
            info = anim.GetCurrentAnimatorStateInfo(0);
            steps++;
            yield return null;
        }

        CleanUp();
    }

    #endregion

    #region Trigger

    // Trigger Enter
    public void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("OntriggerEnter");
        Health health = other.GetComponent<Health>();
        if (health != null)
        {
            health.DoDamage(Damage);
        }
        //Bomb chain
        Bomb bomb = other.GetComponent<Bomb>();
        if (bomb != null)
        {
            bomb.DetonateNow();
        }
    }

    //public void OnTriggerStay2D(Collider2D collision)
    //{
    //    //Debug.Log("OnTriggerStay2D");
    //}

    #endregion

    #region Cleanup

    public void CleanUp()
    {
        StopAllCoroutines();
        GameObject.Destroy(this.gameObject);
    }

    #endregion
}
