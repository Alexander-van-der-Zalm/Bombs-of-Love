using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Node
{
    [System.NonSerialized]
    public List<Node> Neighbors;

    [System.NonSerialized]
    public Node CameFrom;
    public float GScore;
    public float FScore;
    public bool Traversable = true;
    public Vector2 Pos; 

    public Node()
    {
        Neighbors = null;
        CameFrom = null;
    }

    public override string ToString()
    {
        return Pos.ToString() + " G" + GScore.ToString("0.00") + " F" + FScore.ToString("0.00");
    }
}

[System.Serializable]
public class ListNode
{
    public Node this[int i] { get { return Nodes[i]; } set { Nodes[i] = value; } }

    public List<Node> Nodes;

    public ListNode()
    {

    }
}

[System.Serializable]
public class NodeGrid
{
    #region Fields & Properties

    public Node this[int x,int y] { get { return OutOfBounds(x,y) ? null : NodesList[y][x]; } set { if(!OutOfBounds(x, y))NodesList[y][x] = value; } }
    public Node this[Vector2 p] { get { return this[(int)p.x, (int)p.y]; } set { this[(int)p.x, (int)p.y] = value; } }

    public List<ListNode> NodesList;
    public int Height { get; private set; }
    public int Width { get; private set; }

    private bool OutOfBounds(int x, int y)
    {
        return x < 0 || y < 0 || NodesList == null || y >= NodesList.Count || NodesList[y].Nodes == null || x >= NodesList[y].Nodes.Count;
    }

    #endregion

    #region Ctor

    public NodeGrid(int width, int height, List<Node> inputList = null, bool diagonals = true, bool removeUnreachableDiagonalNeighbors = true)
    {
        Height = height;
        Width = width;

        NodesList = new List<ListNode>();
        for (int y = 0; y < height; y++)
        {
            NodesList.Add(new ListNode());
            for (int x = 0; x < width; x++)
            {
                Node nn = null;

                if (inputList != null)
                    nn = inputList.Find(n => n.Pos == new Vector2(x, y));

                if (nn == null) // No node find at coordinate
                {
                    nn = new Node();
                    nn.Pos = new Vector2(x, y);
                }

                if (NodesList[y].Nodes == null)
                    NodesList[y].Nodes = new List<Node>();

                NodesList[y].Nodes.Add(nn);
            }
        }

        CalculateGridNeighbors(diagonals, removeUnreachableDiagonalNeighbors);
    }

    #endregion

    public void CalculateGridNeighbors(bool diagonals = true, bool removeUnreachableDiagonals = true)
    {
        foreach(ListNode l in NodesList)
        {
            foreach(Node n in l.Nodes)
            {
                n.Neighbors = new List<Node>();
                for (int y = -1; y <= 1; y++)
                {
                    for (int x = -1; x <= 1; x++)
                    {
                        // Add neighbors
                        // Skip itself
                        if (x == 0 & y == 0)
                            continue;
                        if(Mathf.Abs(x) + Mathf.Abs(y) == 2) // Diagonal node
                        {
                            if (!diagonals) // Skip diagonals if put off
                                continue;
                        }
                        

                        Vector2 pos = n.Pos + new Vector2(x, y);
                        Node newNeighbor = this[pos];

                        if (newNeighbor != null)
                            n.Neighbors.Add(newNeighbor);
                    }
                }
            }
        }
        if (diagonals && removeUnreachableDiagonals)
        {
            // Remove unreachable diagonal neighbors
            foreach (ListNode l in NodesList)
            foreach (Node n in l.Nodes)
            {
                for (int y = -1; y <= 1; y++)
                for (int x = -1; x <= 1; x++)
                {
                    // Skip itself
                    if (x == 0 & y == 0)
                        continue;
                    if (Mathf.Abs(x) + Mathf.Abs(y) != 2) // Skip non diagonals
                        continue;

                    //Check if both orthogonal neighbors are traversable
                    Node n1 = this[n.Pos + new Vector2(0, y)];
                    Node n2 = this[n.Pos + new Vector2(x, 0)];

                    if ((n1 != null && n1.Traversable) || (n2!= null && n2.Traversable)) // Skip if one neighbor is traversable
                        continue;

                    Vector2 pos = n.Pos + new Vector2(x, y);
                    Node newNeighbor = this[pos];

                    if (newNeighbor != null)
                        n.Neighbors.Remove(newNeighbor);
                }
            }
        }

    }
}