using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// This is all the data that describes a level
/// </summary>
public class GridData : ScriptableObject
{
    public float TileWidth = 1.0f;
    public float TileHeight = 1.0f;

    public GridPrefabList PrefabList;
    //[HideInInspector]
    /// <summary>
    /// Saved to disk & Serialized
    /// </summary>
    public List<GridDataElement> GridDataList;

    /// <summary>
    /// Lives Runtime
    /// </summary>
    [HideInInspector]
    public List<GridDataElement>[,,] GridRuntimeArray;
    private int minX, minY, maxX, maxY;
    private int layerMin, layerMax;

    #region GetSet

    public GridDataElement this[int layer, int x, int y] { get { return GridRuntimeArray[layer, x, y][0]; } }// Check out of bounds
    public GridDataElement this[int layer, int x, int y, int nr] { get { return CheckInArrayBounds(layer,x,y,nr) ? GridRuntimeArray[layer, x, y][nr] : null; } }
    public int Layers { get { return layerMax - layerMin + 1; } }
    public int GridWidth { get { return maxX - minX + 1; } }
    public int GridHeight { get { return maxY - minY + 1; } }

    private bool CheckInArrayBounds(int layer, int x, int y, int nr)
    {
        if(GridRuntimeArray == null)
        {
            Initialize();
        }
        if (x < minX || x > maxX || y < minY || y > maxY)
        {
            Debug.LogError("Coords out of Range");
            return false;
        }
            
        if (layer < layerMin || layer > layerMax)
        {
            Debug.LogError("Layer out of Range");
            return false;
        }
        if(nr >= GridRuntimeArray[layer, x, y].Count)
        {
            Debug.LogError("Nr out of Range");
            return false;
        }

        return true;
    }

    public bool Traversable(int layer, int x, int y){return false; }

    #endregion

    public void Awake()
    {
        // Create RuntimeArray
        Initialize();
    }

    public void Initialize()
    {
        Debug.Log("Initializing Runtime Grid Array");

        #region First find the size of the grid

        if (GridDataList.Count > 0)
        {
            minX = int.MaxValue;
            minY = int.MaxValue;
            maxX = -int.MaxValue;
            maxY = -int.MaxValue;

            foreach (GridDataElement el in GridDataList)
            {
                if (el.X < minX)
                    minX = el.X;
                if (el.Y < minY)
                    minY = el.Y;
                if (el.X > maxX)
                    maxX = el.X;
                if (el.Y > maxY)
                    maxY = el.Y;
            }
        }
        else
        {
            minX = 0;
            minY = 0;
            maxX = 0;
            maxY = 0;
        }

        #endregion

        #region Find the size of the layers
        if (PrefabList.GridLayers.Count > 0)
        {
            layerMin = int.MaxValue;
            layerMax = -int.MaxValue;

            foreach (GridLayer layer in PrefabList.GridLayers)
            {
                if (layer.Layer < layerMin)
                    layerMin = layer.Layer;
                if (layer.Layer < layerMax)
                    layerMax = layer.Layer;
            }
        }
        else
        {
            layerMin = 0;
            layerMax = 0;
        }
        #endregion

        // Initialize array
        GridRuntimeArray = new List<GridDataElement>[Layers, GridWidth, GridHeight];

        foreach (GridDataElement el in GridDataList)
        {
            GridRuntimeArray[el.Prefab.GridLayer, el.X, el.Y].Add(el);
        }
    }
}
