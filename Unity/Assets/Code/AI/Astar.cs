﻿using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

[System.Serializable]
public class Astar
{
    public static List<Node> FindPath(Node start, Node goal)
    {
        List<Node> OpenList = new List<Node>();
        List<Node> ClosedList = new List<Node>();

        //int randomID = UnityEngine.Random.Range(0, int.MaxValue);

        start.CameFrom = null;
        start.GScore = 0;
        start.FScore = HeuristicScore(start, goal);

        OpenList.Add(start);

        while(OpenList.Count != 0)
        {
            // Select the current node with the lowest F Score 
            // ######### (Prio Q plx) ##########
            Node current = OpenList[0];
            for(int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].FScore < current.FScore)
                    current = OpenList[i];
            }

            // Goal has been found
            if (current == goal)
                return ReturnPath(start, goal);

            OpenList.Remove(current);
            ClosedList.Add(current);

            foreach(Node n in current.Neighbors)
            {
                if (ClosedList.Contains(n))
                    continue; // Already evaluated

                float nGscore = current.GScore + DistanceBetween(current, n);
                if (!OpenList.Contains(n)) 
                    OpenList.Add(n); // New node
                else if (nGscore >= n.GScore)
                    continue; // Path is not better

                n.GScore = nGscore;
                n.FScore = nGscore + HeuristicScore(n, goal);
                n.CameFrom = current;
            }
        }

        // Failure
        return new List<Node>();
    }

    private static float HeuristicScore(Node a, Node b)
    {
        Vector2 d = a.Pos - b.Pos;
        return Mathf.Abs(d.x) + Mathf.Abs(d.y);
    }

    private static float DistanceBetween(Node a, Node b)
    {
        // Could be optimized probably
        return Mathf.Abs((a.Pos - b.Pos).magnitude);
    }

    private static List<Node> ReturnPath(Node start, Node goal)
    {
        List<Node> path = new List<Node>();
        
        // Add the startnode at pos 0
        path.Add(start);

        // 
        Node current = goal;
        while (current != start)
        {
            path.Insert(1, current);
            current = current.CameFrom;
        }

        return path;
    }
}

// Priority Queue idea:
// Dic<priority,
