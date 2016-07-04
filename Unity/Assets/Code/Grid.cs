using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class Grid : MonoBehaviour
{
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

    public Vector2 GetCurrentGridPos(Vector3 worldLocation)
    {
        // Calculate
        int x = (int) Mathf.Floor((worldLocation.x - this.transform.position.x) / GridWidth);
        int y = (int)Mathf.Floor((worldLocation.y - this.transform.position.y) / GridHeight);

        return new Vector2(Mathf.Floor((worldLocation.x-this.transform.position.x)/GridWidth), Mathf.Floor((worldLocation.y - this.transform.position.y) / GridHeight));
    }

    //public static GridElement RayCastElement(Vector3 worldLocation)
    //{

    //}

    public Vector3 GetGridWorldPos(int x, int y)
    {
        return gridArray[x, y].transform.position;
    }

    public GridElement GetGridElement(int x, int y)
    {
        //Debug.Log("x: " + x + " y: " + y);
        //Debug.Log("element: " + gridArray[x, y]);
        return gridArray[x, y];
    }

    #endregion
}
