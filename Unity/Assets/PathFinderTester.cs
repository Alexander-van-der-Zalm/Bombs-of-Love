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
    public bool Diagonals = true;
    private bool oldDiagonals = true;

    private Vector2 lastStart, lastGoal;

    public void Awake()
    {
        INit();
    }

    private void INit()
    {
        Nodes.CreateNodesGrid(Width, Height, Diagonals);
        Nodes[1, 1].Traversable = false;
        Nodes[2, 1].Traversable = false;
        Nodes[3, 1].Traversable = false;
        Nodes[4, 1].Traversable = false;
        Nodes[0, 3].Traversable = false;
        Nodes[1, 3].Traversable = false;
        Nodes[2, 3].Traversable = false;
        Nodes[3, 3].Traversable = false;
        Nodes[4, 3].Traversable = false;
        Nodes[5, 3].Traversable = false;

        oldDiagonals = Diagonals;
    }

    public void Update()
    {
        if(Diagonals != oldDiagonals)
        {
            INit();
            Path = Astar.FindPath(Nodes[Start], Nodes[Goal]);
        }
        if (lastStart != Start || lastGoal != Goal)
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
        // Draw blocks
        for (int y = 0; Nodes != null && y < Nodes.Height; y++)
            for (int x = 0; x < Nodes.Width; x++)
            {
                if (!Nodes[x, y].Traversable)
                    Gizmos.DrawCube(Nodes[x, y].Pos + offset, new Vector3(0.8f, 0.8f, 0.8f));
            }
    }
}
