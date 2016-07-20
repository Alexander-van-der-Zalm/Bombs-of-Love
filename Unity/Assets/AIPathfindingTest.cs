using UnityEngine;
using System.Collections;

public class AIPathfindingTest : MonoBehaviour
{
    public AgentFollowPath FollowPath;
    public PathFinderTester Tester;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (FollowPath.Path != Tester.Path)
            FollowPath.Path = Tester.Path;
	}
}
