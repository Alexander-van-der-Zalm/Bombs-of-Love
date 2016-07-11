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
        Horizontal = Axis.Default(AxisKey.HorVert.Horizontal);
    }

    public void Update()
    {
        Debug.Log(Horizontal.Value());
    }
}
