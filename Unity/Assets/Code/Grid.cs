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

    [Range(0.0f,1.0f)]
    public float BlockChance = 0.3f;

    public static float TileWidth = 1.0f;
    public static float TileHeight = 1.0f;

    [SerializeField]
    public GridElement[,] levelArray;

    [SerializeField]
    private Array2D<GridElement> blockArray;

    public int GridHeight { get { return 2 + rows * 2 + 1; } }
    public int GridWidth { get { return 2 + columns * 2 + 1; } }

    private GameObject levelContainer;
    private GameObject blockContainer;

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

        levelArray = new GridElement[width, height];

        #region Walls
        // Top
        for(int i = 0; i < width; i++)
            Create(i, 0, Wall, levelContainer, "levelContainer");
        // Bottom
        for (int i = 0; i < width; i++)
            Create(i, height-1, Wall, levelContainer, "levelContainer");
        // Side 1
        for (int i = 1; i < height-1; i++)
            Create(0, i, Wall, levelContainer, "levelContainer");
        // Side 1
        for (int i = 1; i < height - 1; i++)
            Create(width -1, i, Wall, levelContainer, "levelContainer");
        #endregion

        #region Floors & inner pillars

        // Full floors
        for (int j = 1; j < height - 1; j++)
            for (int i = 1; i < width -1; i += 2)
                Create(j, i, Floor, levelContainer, "levelContainer");

        for (int j = 1; j < height - 1; j++)
            for (int i = 2; i < width - 2; i +=2)
            {
                if(j%2==0)
                    Create(j, i, InnerWall, levelContainer, "levelContainer");
                else
                    Create(j, i, Floor, levelContainer, "levelContainer");
            }


        #endregion

        DebugArray();
    }

    public void GenerateBlocks()
    {
        // Loop randomly over the grid with a randomChance to fill the floor tiles with a destructable block
        for(int y = 0; y < GridHeight; y++)
            for (int x = 0; x < GridWidth; x++)
            {
                GridElement el = levelArray[x, y];
                if (el.Type == GridElement.GridType.Floor 
                    && UnityEngine.Random.Range(0f, 1.0f) <= BlockChance)
                {
                    Create(x, y, Block, blockContainer, "blockContainer");
                }
            }

        // Clear near the corners

    }


    public void RestoreGridArray()
    {
        // Make a new grid
        int height = 2 + rows * 2 + 1; // 2 rows of walls + 2 per row + 1 to finish
        int width = 2 + columns * 2 + 1;

        levelArray = new GridElement[width, height];

        List<GridElement> elements = GetComponentsInChildren<GridElement>().ToList();
        foreach (GridElement el in elements)
        {
            levelArray[el.x, el.y] = el;
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
                Debug.Log("x: " + x + " y: " + y + " named: " + levelArray[x, y].name);
            }
    }

    private void Create(int x, int y, GameObject obj, GameObject container, string containerName)
    {
        Vector3 pos = new Vector3(x * TileWidth, y * TileHeight) + transform.position;
        GameObject newObj = GameObject.Instantiate(obj, pos, Quaternion.identity) as GameObject;
        levelArray[x, y] = newObj.GetComponent<GridElement>();
        levelArray[x, y].x = x; //dunno if this is needed
        levelArray[x, y].y = y;
        levelArray[x, y].ParentGrid = this;

        if(container == null)
        {
            container = new GameObject();
            container.name = containerName;
            container.transform.parent = this.transform;
            container.transform.position = Vector3.zero;
        }
        newObj.transform.parent = levelContainer.transform;
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
        return new Vector2(Mathf.Floor((worldLocation.x-this.transform.position.x)/TileWidth), Mathf.Floor((worldLocation.y - this.transform.position.y) / TileHeight));
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
        return levelArray[x, y].transform.position + SnapOffset(offset);
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
                return new Vector3(TileWidth / 2, 0);

            case SnapSpot.Right:
                return new Vector3(TileWidth, 0);
        }
    }

    #endregion

    #region Element

    public GridElement GetGridElementFromWorld(Vector3 worldPos)
    {
        Vector2 p = GetGridCoordinates(worldPos);
        return levelArray[(int)p.x, (int)p.y];
    }

    public GridElement GetGridElement(Vector2 gridCoord)
    {
        return levelArray[(int)gridCoord.x, (int)gridCoord.y];
    }

    public GridElement GetGridElement(int x, int y)
    {
        return levelArray[x, y];
    }

    #endregion

    #endregion
}
