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



    public PlayerIndex XboxControllerIndex;

    private MovementPhysics physics;
    private GamePadState prevState;
    private GamePadState state;

    // Use this for initialization
    void Start ()
    {
        physics = GetComponent<MovementPhysics>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        prevState = state;
        state = GamePad.GetState(XboxControllerIndex);
        
    }
}
