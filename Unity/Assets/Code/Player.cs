using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour
{
    #region fields

    public int Lives = 3;
    public PlayerSpawner spawn;
    public bool InfiniteLives = false;

    private Health health;
    private PlayerAnimationHandler animationHandler;
    private Rigidbody2D rb;
    private Animator anim;
    private Bomber bomber;
    private Invulnerability inv;

    private int animDeadHash = Animator.StringToHash("Dead");

    #endregion

    #region Awake

    void Awake()
    {
        health = GetComponent<Health>();
        animationHandler = GetComponent<PlayerAnimationHandler>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        bomber = GetComponent<Bomber>();
        inv = GetComponent<Invulnerability>();

        Respawn(false,false);
    }

    #endregion

    #region Death

    public void Death()
    {
        Debug.Log("Death");
        Lives--;
        animationHandler.Die();

        // Stop old invulner
        inv.InvulnerableReset();
        StopAllCoroutines();

        // Respawn if there are still lives left
        if (Lives > 0 || InfiniteLives)
            StartCoroutine(WaitForRespawn());
        //else
        //    GameOver(); ??
    }

    #endregion

    #region Respawn

    private void Respawn(bool anim = true, bool invul = true)
    {
        Debug.Log("Respawn");
        if(anim)
            animationHandler.Respawn();

        // Reset health & bomber status
        health.Respawn();
        bomber.OnBomb = false;

        // Place rigidbody on right location
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        rb.position = spawn.transform.position;

        // Start invulnerability
        if (invul)
            inv.StartInvulnerability();
    }

    private IEnumerator WaitForRespawn()
    {
        // Wait for animation to be finished
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        while (info.shortNameHash != animDeadHash)
        {
            info = anim.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }

        Respawn();
    }

    #endregion
}
