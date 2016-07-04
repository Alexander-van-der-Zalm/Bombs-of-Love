using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    #region fields

    public int Lives = 3;
    public PlayerSpawner spawn;

    private Health health;
    private PlayerAnimationHandler animationHandler;
    private Rigidbody2D rb;
    private Animator anim;

    private int animDeadHash = Animator.StringToHash("Dead");

    #endregion

    #region Awake

    void Awake()
    {
        health = GetComponent<Health>();
        animationHandler = GetComponent<PlayerAnimationHandler>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    #endregion

    #region Death

    public void Death()
    {
        Debug.Log("Death");
        Lives--;
        animationHandler.Die();

        if (Lives > 0)
            StartCoroutine(WaitForRespawn());
        //else
        //    GameOver();
    }

    #endregion

    #region Respawn

    private void Respawn()
    {
        Debug.Log("Respawn");
        animationHandler.Respawn();
        health.Respawn();

        //rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        rb.position = spawn.transform.position;
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
