using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class Grid : MonoBehaviour
{
    public enum SnapSpot
    {
        Left,Mid,Right
    }
    
    #region Fields

    public GameObject Floor;
    public GameObject Wall;
    public GameObject InnerWall;
    public GameObject Block;

    public int rows = 6;
    public int columns = 6;
    public static float GridWidth = 1.0f;
    public static float GridHeight = 1.0f;

    [SerializeField]
    public GridElement[,] gridArray;

    public int GridRows { get { return 2 + rows * 2 + 1; } }
    public int GridColumns { get { return 2 + columns * 2 + 1; } }

    #endregion

    public void Start()
    {
        RestoreGridArray();
    }

    #region Generate

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
                Create(j, i, Floor);

        for (int j = 1; j < height - 1; j++)
            for (int i = 2; i < width - 2; i +=2)
            {
                if(j%2==0)
                    Create(j, i, InnerWall);
                else
                    Create(j, i, Floor);
            }


        #endregion

        DebugArray();
    }

    public void RestoreGridArray()
    {
        // Make a new grid
        int height = 2 + rows * 2 + 1; // 2 rows of walls + 2 per row + 1 to finish
        int width = 2 + columns * 2 + 1;

        gridArray = new GridElement[width, height];

        List<GridElement> elements = GetComponentsInChildren<GridElement>().ToList();
        foreach (GridElement el in elements)
        {
            gridArray[el.x, el.y] = el;
        }
    }

    public void DebugArray()
    {
        // Make a new grid
        int height = 2 + rows * 2 + 1; // 2 rows of walls + 2 per row + 1 to finish
        int width = 2 + columns * 2 + 1;

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                Debug.Log("x: " + x + " y: " + y + " named: " + gridArray[x, y].name);
            }
    }

    private void Create(int x, int y, GameObject obj)
    {
        Vector3 pos = new Vector3(x * GridWidth, y * GridHeight) + transform.position;
        GameObject newObj = GameObject.Instantiate(obj, pos, Quaternion.identity) as GameObject;
        gridArray[x, y] = newObj.GetComponent<GridElement>();
        gridArray[x, y].x = x; //dunno if this is needed
        gridArray[x, y].y = y;
        gridArray[x, y].ParentGrid = this;
        newObj.transform.parent = this.transform;

    }

    #endregion

    #region Delete

    public void DeleteGrid()
    {
        int childCount = transform.childCount;
        
        for(int i = 0; i < childCount; i++)
            GameObject.DestroyImmediate(transform.GetChild(0).gameObject);
    }

    #endregion

    #region Get

    /// <summary>
    /// Returns x & y grid coordinate
    /// </summary>
    public Vector2 GetGridCoordinates(Vector3 worldLocation)
    {
        return new Vector2(Mathf.Floor((worldLocation.x-this.transform.position.x)/GridWidth), Mathf.Floor((worldLocation.y - this.transform.position.y) / GridHeight));
    }

    //public Vector3 WorldToLeftBottomSnappedGridPos(Vector3 worldLocation)
    //{
    //    return GetGridWorldPos(GetGridPos(worldLocation));
    //}

    //public Vector3 WorldToMidBottomSnappedGridPos(Vector3 worldLocation)
    //{
    //    return GetGridWorldPos(GetGridPos(worldLocation)) + new Vector3(GridWidth/2,0);
    //}

    public Vector3 WorldToSnappedGridPos(Vector3 worldLocation, SnapSpot offset = SnapSpot.Left)
    {
        return GetGridWorldPos(GetGridCoordinates(worldLocation), offset);
    }

    public Vector3 GetGridWorldPos(Vector2 gridCoord, SnapSpot offset = SnapSpot.Left)
    {
        return GetGridWorldPos((int)gridCoord.x, (int)gridCoord.y, offset);// gridArray[(int)gridPos.x, (int)gridPos.y].transform.position + SnapOffset(offset);
    }

    public Vector3 GetGridWorldPos(int x, int y, SnapSpot offset = SnapSpot.Left)
    {
        return gridArray[x, y].transform.position + SnapOffset(offset);
    }

    #region offset

    private Vector3 SnapOffset(SnapSpot offset)
    {
        switch (offset)
        {
            default:
            case SnapSpot.Left:
                return Vector3.zero;

            case SnapSpot.Mid:
                return new Vector3(GridWidth / 2, 0);

            case SnapSpot.Right:
                return new Vector3(GridWidth, 0);
        }
    }

    #endregion

    #region Element

    public GridElement GetGridElementFromWorld(Vector3 worldPos)
    {
        Vector2 p = GetGridCoordinates(worldPos);
        return gridArray[(int)p.x, (int)p.y];
    }

    public GridElement GetGridElement(Vector2 gridCoord)
    {
        return gridArray[(int)gridCoord.x, (int)gridCoord.y];
    }

    public GridElement GetGridElement(int x, int y)
    {
        return gridArray[x, y];
    }

    #endregion

    #endregion
}
