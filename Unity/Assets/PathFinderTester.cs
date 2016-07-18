using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinderTester : MonoBehaviour
{
    public int Width, Height;
    public List2DNode Nodes;
    public List<Node> Path;

    public Transform StartTr, GoalTr;

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

        for (int y = 0; Nodes != null && y < Nodes.Height; y++)
            for (int x = 0; x < Nodes.Width; x++)
            {
                Nodes[x, y].Traversable = Random.Range(0,1.0f)>0.4f;

            }
        //Nodes[2, 1].Traversable = false;
        //Nodes[3, 1].Traversable = false;
        //Nodes[4, 1].Traversable = false;
        //Nodes[0, 3].Traversable = false;
        //Nodes[1, 3].Traversable = false;
        //Nodes[2, 3].Traversable = false;
        //Nodes[3, 3].Traversable = false;
        //Nodes[4, 3].Traversable = false;
        //Nodes[5, 3].Traversable = false;

        oldDiagonals = Diagonals;
    }

    public void Update()
    {
        if (StartTr != null)
            Start = new Vector2((int)StartTr.position.x, (int)StartTr.position.y);
        if (GoalTr != null)
            Goal = new Vector2((int)GoalTr.position.x, (int)GoalTr.position.y);
        if (Diagonals != oldDiagonals)
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

        if (StartTr != null)
            Start = new Vector2((int)StartTr.position.x, (int)StartTr.position.y);
        if (GoalTr != null)
            Goal = new Vector2((int)GoalTr.position.x, (int)GoalTr.position.y);

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
        foreach(Node n in Path)
        {
            Gizmos.DrawSphere(n.Pos + offset, 0.1f);
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
