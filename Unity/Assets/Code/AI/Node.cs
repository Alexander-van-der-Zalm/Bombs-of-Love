using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Node
{
    public List<Node> Neighbors;
    public Node CameFrom;
    public float GScore;
    public float FScore;
    public int SearchID;
    public Vector2 Pos;
}
