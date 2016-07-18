using UnityEngine;
using System.Collections;

public class TestControlScheme : MonoBehaviour
{
    public InputAction Jump;
    public InputAction DropBomb;

    public InputAxis Horizontal;
    public InputActionKey Key;
    public InputAxisKey axis;

    public Vector3 V3;

    public void Awake()
    {
        //Jump.Update
        //Horizontal = Axis.Default(DirectionInput.Horizontal);
        Jump = InputAction.Create("Space", XboxButton.A);
    }

    public void Update()
    {
        Debug.Log(Horizontal.Value() +  " Jump " +  Jump.IsDown());

    }
}
