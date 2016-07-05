using UnityEngine;
using System.Collections;

public class MovementPhysics : MonoBehaviour
{
    #region Fields

    public float Speed = 1.0f;

    private Transform tr;
    private Rigidbody2D rb;

    private float InputHorizontal = 0;
    private float InputVertical = 0;

    public float InputHor { get { return InputHorizontal; } }
    public float InputVer { get { return InputVertical; } }

    #endregion

    // Use this for initialization
    void Start ()
    {
        tr = this.transform;
        rb = this.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {

        rb.velocity = new Vector2(InputHorizontal, InputVertical).normalized * Speed * Time.fixedDeltaTime * 50;
        //tr.position += new Vector3(InputHorizontal, InputVertical,0).normalized * Speed * Time.fixedDeltaTime;

        if (InputHorizontal == 0 && InputVertical == 0)
            rb.velocity = Vector2.zero;
    }

    public void SetMovementInput(float hor, float ver)
    {
        InputHorizontal = hor;
        InputVertical = ver;
    }
}
