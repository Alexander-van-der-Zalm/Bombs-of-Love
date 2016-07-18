using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinderTester : MonoBehaviour
{
    public int Width, Height;
    public List2DNode Nodes;
    public List<Node> Path;

    public GridLineDrawer drawer;
    public Vector2 Start, Goal;

    private Vector2 lastStart, lastGoal;

    public void Awake()
    {
        Nodes.CreateNodesGrid(Width, Height, null);
    }


    public void Update()
    {
        if(lastStart != Start || lastGoal != Goal)
        {
            Path = Astar.FindPath(Nodes[Start], Nodes[Goal]);
            lastGoal = Goal;
            lastStart = Start;
            Debug.Log("PathFound");
        }
    }

    
    public void OnDrawGizmos()
    {
        Vector2 offset = new Vector2(0.5f, 0.5f);
        Gizmos.DrawWireSphere(Start + offset, 0.5f);
        Gizmos.DrawWireSphere(Goal + offset, 0.5f);
       
        // Draw Grid
        if (drawer.DrawLinesInEditor)
        {
            drawer.DrawGrid(Width, Height, 1, 1, Vector3.zero, drawer.GridColor); // potentially change to the size of the camerawidth
        }

        // Draw Path
        for(int i = 0; Path != null && i < Path.Count-1;i++)
        {
            Gizmos.DrawLine(Path[i].Pos + offset, Path[i + 1].Pos + offset);
        }
    }
}
