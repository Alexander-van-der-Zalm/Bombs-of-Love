using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IPathFinder
{
    List<Node> FindPath(Node start, Node goal);
}