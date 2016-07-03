using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Grid : MonoBehaviour
{
    public GameObject Floor;
    public GameObject Wall;
    public GameObject InnerWall;
    public GameObject Block;

    public int rows = 6;
    public int columns = 6;
    public float GridWidth = 1.0f;
    public float GridHeight = 1.0f;

    private GridElement[,] gridArray;

    public void GenerateGrid()
    {
        // Destroy old grid
        DeleteGrid();

        // Make a new grid
        int height = 2 + rows * 2 + 1; // 2 rows of walls + 2 per row + 1 to finish
        int width = 2 + columns * 2 + 1;

        gridArray = new GridElement[width, height];

        #region Walls
        // Top
        for(int i = 0; i < width; i++)
            Create(i, 0, Wall);
        // Bottom
        for (int i = 0; i < width; i++)
            Create(i, height-1, Wall);
        // Side 1
        for (int i = 1; i < height-1; i++)
            Create(0, i, Wall);
        // Side 1
        for (int i = 1; i < height - 1; i++)
            Create(width -1, i, Wall);
        #endregion

        #region Floors & inner pillars

        // Full floors
        for (int j = 1; j < height - 1; j++)
            for (int i = 1; i < width -1; i += 2)
                Create(i, j, Floor);

        for (int j = 1; j < height - 1; j++)
            for (int i = 2; i < width - 2; i +=2)
            {
                if(j%2==0)
                    Create(i, j, InnerWall);
                else
                    Create(i, j, Floor);
            }
                

        #endregion
    }

    private void Create(int x, int y, GameObject obj)
    {
        Vector3 pos = new Vector3(x * GridWidth, y * GridHeight);
        GameObject newObj = GameObject.Instantiate(obj, pos, Quaternion.identity) as GameObject;
        gridArray[x, y] = newObj.GetComponent<GridElement>();
        newObj.transform.parent = this.transform;
    }

    public void DeleteGrid()
    {
        int childCount = transform.childCount;
        
        for(int i = 0; i < childCount; i++)
            GameObject.DestroyImmediate(transform.GetChild(0).gameObject);
    }
}
