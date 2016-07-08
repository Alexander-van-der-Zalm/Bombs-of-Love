using UnityEngine;
using System.Collections;

public class PlayerAnimationHandler : MonoBehaviour
{
    private int deathTriggerH = Animator.StringToHash("Death");
    private int respawnTriggerH = Animator.StringToHash("Respawn");
    private int velocityH = Animator.StringToHash("Velocity");
    private int velocityH_X = Animator.StringToHash("Velocity_X");
    private int velocityH_Y = Animator.StringToHash("Velocity_Y");
    private int facingH_X = Animator.StringToHash("LastFacing_X");
    private int facingH_Y = Animator.StringToHash("LastFacing_Y");

    private Animator anim;
    private Rigidbody2D rb;
    private MovementPhysics physics;
    private Health health;

    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        physics = GetComponent<MovementPhysics>();
        health = GetComponent<Health>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        Vector2 v = rb.velocity.normalized;
        float speed = rb.velocity.magnitude / physics.Speed;
        
        anim.SetFloat(velocityH, speed);
        anim.SetFloat(velocityH_X, physics.InputHor);
        anim.SetFloat(velocityH_Y, physics.InputVer);

        if (physics.InputHor !=0 || physics.InputVer != 0)
        {
            anim.SetFloat(facingH_X, physics.InputHor);
            anim.SetFloat(facingH_Y, physics.InputVer);
        }
    }

    public void Die()
    {
        anim.SetTrigger(deathTriggerH);
    }

    public void Respawn()
    {
        anim.SetTrigger(respawnTriggerH);
    }
}
