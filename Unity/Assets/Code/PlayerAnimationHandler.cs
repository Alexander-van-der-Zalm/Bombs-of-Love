using UnityEngine;
using System.Collections;

public class PlayerAnimationHandler : MonoBehaviour
{
    private int deathTriggerH = Animator.StringToHash("Death");
    private int velocityH = Animator.StringToHash("Velocity");
    private int velocityH_X = Animator.StringToHash("Velocity_X");
    private int velocityH_Y = Animator.StringToHash("Velocity_Y");

    private Animator anim;
    private Rigidbody2D rb;
    private MovementPhysics physics;

    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        physics = GetComponent<MovementPhysics>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        Vector2 v = rb.velocity.normalized;
        anim.SetFloat(velocityH, rb.velocity.magnitude/physics.Speed);
        anim.SetFloat(velocityH_X, v.x);
        anim.SetFloat(velocityH_Y, v.y);
    }

    public void Death()
    {
        anim.SetTrigger(deathTriggerH);
    }
}
