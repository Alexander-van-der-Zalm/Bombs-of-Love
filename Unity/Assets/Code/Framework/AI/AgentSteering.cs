using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

public class AgentSteering : MonoBehaviour
{
    public Transform Target;

    private IMovementPhysics movePhysics;
    private Transform tr;

    // Use this for initialization
    void Start ()
    {
        Debug.Log("Start");
        movePhysics = GetComponent<IMovementPhysics>();
        tr = transform;

        if(Target == null)
        {
            Target = new GameObject("AI target").transform;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector2 dir = Target.position - tr.position;
        dir.Normalize();
        movePhysics.SetMovementInput(dir.x, dir.y);
	}
}
