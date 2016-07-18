using UnityEngine;
using System.Collections;
using XInputDotNetPure;

[RequireComponent(typeof(MovementPhysics))]
public class InputHandler : MonoBehaviour
{
    public PlayerIndex PlayerIndex;
    public InputAxis Horizontal, Vertical;
    public InputAction DropBomb;

    private MovementPhysics physics;
    private Bomber bomber;
    private BMGrid grid;
    private Health health;

    // Use this for initialization
    void Start ()
    {
        physics = GetComponent<MovementPhysics>();
        bomber = GetComponent<Bomber>();
        grid = GameObject.FindObjectOfType<BMGrid>();
        health = GetComponent<Health>();

        if (DropBomb == null)
            DropBomb = InputAction.Create(KeyCode.Space, XboxButton.A, PlayerIndex);
        if (Horizontal == null)
            Horizontal = InputAxis.Default(DirectionInput.Horizontal, PlayerIndex);
        if (Vertical == null)
            Vertical = InputAxis.Default(DirectionInput.Vertical, PlayerIndex);

        // Set XboxPlayerIndex
        DropBomb.PlayerIndex = this.PlayerIndex;
        Horizontal.PlayerIndex = this.PlayerIndex;
        Vertical.PlayerIndex = this.PlayerIndex;
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
            hor = Horizontal.Value();
            ver = Vertical.Value();
        }
        physics.SetMovementInput(hor, ver);
    }

    // Actions
    void Update()
    {
        if (!health.IsDead && GameState.Instance.State == GameState.GameStateEnum.Play && DropBomb.IsReleased())
            bomber.DropBomb(grid, transform.position);
    }
}
