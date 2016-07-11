using UnityEngine;
using System.Collections;

public class TestControlScheme : MonoBehaviour
{
    public Action Jump;
    public Action DropBomb;

    public Axis Horizontal;

    public void Awake()
    {
        //Jump.Update
        Horizontal = Axis.Default(AxisKey.DirectionInput.Horizontal);
        Jump = Action.Create("A", XboxButton.A);
    }

    public void Update()
    {
        Debug.Log(Jump.IsDown());

    }
}
