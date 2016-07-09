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
    public enum ExplosionSection
    {
        End, Mid, Center
    }

    #endregion

    #region Fields

    public ExplosionRotation Rotation;
    public ExplosionSection Section;

    public int Damage;

    private Animator anim;
    private int animRotation = Animator.StringToHash("Rotation");
    private int animType = Animator.StringToHash("Type");
    private int animFinishedState = Animator.StringToHash("Finished");

    private Bomb parentBomb;

    #endregion

    #region Awake

    public void Awake()
    {
        anim = GetComponent<Animator>();
        GameState.Instance.EventHookups.OnGameOver.AddListener(CleanUp);
    }

    #endregion

    #region Start

    public void Initiate(ExplosionSection type, ExplosionRotation rotation, int damage, Bomb parentBomb = null)
    {
        //Debug.Log("I am type: " + type + " rot " + rotation);
        Section = type;
        Rotation = rotation;
        Damage = damage;
        this.parentBomb = parentBomb;

        // Set anim
        anim.SetInteger(animRotation, (int)Rotation);
        anim.SetInteger(animType, (int)Section);

        // Start CleanupCR
        StartCoroutine(CleanupCR());
    }

    private IEnumerator CleanupCR()
    {
        int steps = 0;

        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);

        // Wait to go out of finished anim state (in case it is still in finished)
        while (info.shortNameHash == animFinishedState)
        {
            info = anim.GetCurrentAnimatorStateInfo(0);
            steps++;
            yield return null;
        }

        // Then wait to go back to finished anim state
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

    #endregion

    #region Cleanup

    public void CleanUp()
    {
        if (parentBomb != null)
            parentBomb.UnRegisterExplosion(this);
        StopAllCoroutines();
        GameObject.Destroy(this.gameObject);
    }

    #endregion
}
