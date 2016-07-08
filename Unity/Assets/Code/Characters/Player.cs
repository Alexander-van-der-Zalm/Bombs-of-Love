using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour
{
    #region fields

    public int Lives = 3;
    public PlayerSpawner spawn;
    public bool InfiniteLives = false;
    public AudioClip OnDeathFX;

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

        // Add self to GameLogic
        GameLogic.Instance.Players.Add(this);
        GameState.Instance.EventHookups.OnGameStart.AddListener(PlayerStartSpawn);
    }

    public void PlayerStartSpawn()
    {
        Respawn(false, false);
        if(GameLogic.Instance.Rules.LoseUpgradesOnNewRound)
        {
            bomber.BonusDamage = GameLogic.Instance.Rules.StartBonusDamage;
            bomber.BonusRange = GameLogic.Instance.Rules.StartBonusRange;
            bomber.MaxBombs = GameLogic.Instance.Rules.StartBombs;
            bomber.AvailableBombs = bomber.MaxBombs;
        }
    }

    #endregion

    #region Death

    public void Death()
    {
        Debug.Log("Death");
        Lives--;
        animationHandler.Die();
        AudioSource.PlayClipAtPoint(OnDeathFX, transform.position);
        
        // Stop old invulner
        inv.InvulnerableReset();
        StopAllCoroutines();

        // Respawn if there are still lives left
        if (Lives > 0 || InfiniteLives)
            StartCoroutine(WaitForRespawn());
        else
            GameLogic.Instance.CheckForGameOver();
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
