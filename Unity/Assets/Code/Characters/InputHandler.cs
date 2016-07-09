using UnityEngine;
using System.Collections;
using XInputDotNetPure;

[RequireComponent(typeof(MovementPhysics))]
public class InputHandler : MonoBehaviour
{
    

    public KeyCode Up = KeyCode.W;
    public KeyCode Left = KeyCode.A;
    public KeyCode Down = KeyCode.S;
    public KeyCode Right = KeyCode.D;

    public KeyCode DropBomb = KeyCode.Space;

    //public PlayerIndex XboxControllerIndex;

    private MovementPhysics physics;
    private Bomber bomber;
    private Grid grid;
    private Health health;
    //private GamePadState prevState;
    //private GamePadState state;

    // Use this for initialization
    void Start ()
    {
        physics = GetComponent<MovementPhysics>();
        bomber = GetComponent<Bomber>();
        grid = GameObject.FindObjectOfType<Grid>();
        health = GetComponent<Health>();
    }
	
	// Physics
	void FixedUpdate ()
    {
        //prevState = state;
        //state = GamePad.GetState(XboxControllerIndex);

        float hor = 0;
        float ver = 0;

        if (!health.IsDead && GameState.Instance.State == GameState.GameStateEnum.Play)
        {
            if (Input.GetKey(Up))
                ver++;
            if (Input.GetKey(Down))
                ver--;
            if (Input.GetKey(Left))
                hor--;
            if (Input.GetKey(Right))
                hor++;
        }
        physics.SetMovementInput(hor, ver);
    }

    // Actions
    void Update()
    {
        if (!health.IsDead && GameState.Instance.State == GameState.GameStateEnum.Play && Input.GetKeyUp(DropBomb))
            bomber.DropBomb(grid, transform.position);
    }
}
