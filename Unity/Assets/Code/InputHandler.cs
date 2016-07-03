using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MovementPhysics))]
public class InputHandler : MonoBehaviour
{
    public KeyCode Left, Right, Up, Down;

    private MovementPhysics physics;

	// Use this for initialization
	void Start ()
    {
        physics = GetComponent<MovementPhysics>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}
}
