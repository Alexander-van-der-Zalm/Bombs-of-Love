using UnityEngine;
using System.Collections;

public class TestControlScheme : MonoBehaviour
{
    public Action Jump;
    public Action DropBomb;

    public Axis Horizontal;
    public ActionKey Key;

    public void Awake()
    {
        //Jump.Update
        Horizontal = Axis.Default(DirectionInput.Horizontal);
        Jump = Action.Create("Space", XboxButton.A);
    }

    public void Update()
    {
        Debug.Log(Horizontal.Value() +  " Jump " +  Jump.IsDown());

    }
}
