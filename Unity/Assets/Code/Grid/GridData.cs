using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
/// This is all the data that describes a level
/// </summary>
[CreateAssetMenu]
public class GridData : ScriptableObject
{
    #region Fields

    public float TileWidth = 1.0f;
    public float TileHeight = 1.0f;

    public GridPrefabList PrefabList;
    //[HideInInspector]
    /// <summary>
    /// Saved to disk & Serialized
    /// </summary>
    public List<GridDataElement> GridSaveData;

    public List<ListNode> Node2DList;

    /// <summary>
    /// Lives Runtime
    /// </summary>
    [SerializeField, HideInInspector]
    private List<GridDataElement>[,,] m_GridRuntimeArray;

    private int m_minX, m_minY, m_maxX, m_maxY;
    private int m_LayerMin, m_LayerMax;

    [SerializeField, ReadOnly]
    private int m_Changes = 0;
    [SerializeField, ReadOnly]
    private int m_ArrayInitOnChanges = -1;

    private bool m_ArraySynced { get { return m_GridRuntimeArray != null && m_Changes == m_ArrayInitOnChanges; } }

    #endregion

    #region GetSet

    public List<GridDataElement> this[int layer, int x, int y]
    {
        get { return CheckInArrayBounds(layer, x, y) ? GridRuntimeArray(layer, x, y) : null; }
        private set { if (CheckInArrayBounds(layer, x, y)) SetGridRuntimeArray(layer, x, y, value); }
    }// Check out of bounds
    
    //public List<GridDataElement> this[int layer, int x, int y, int nr] { get { return CheckInArrayBounds(layer,x,y,nr) ? GridRuntimeArray(layer, x, y)[nr] : null; } }
    public int Layers { get { return m_LayerMax - m_LayerMin + 1; } }
    public int GridWidth { get { return m_maxX - m_minX + 1; } }
    public int GridHeight { get { return m_maxY - m_minY + 1; } }

    public int ChangeLog { get { return m_Changes; } }

    private List<GridDataElement> GridRuntimeArray(int layer, int x, int y)
    {
        return m_GridRuntimeArray[layer - m_LayerMin, x - m_minX, y - m_minY];
    }

    private void SetGridRuntimeArray(int layer, int x, int y, List<GridDataElement> p)
    {
        m_GridRuntimeArray[layer - m_LayerMin, x - m_minX, y - m_minY] = p;
    }

    // ########## REMOVE ############
    public List<GridDataElement> GetListAtCoord(int layer, int x, int y)
    {
        return CheckInArrayBounds(layer, x, y) ? GridRuntimeArray(layer, x, y) : null;
    }

    private bool CheckInArrayBounds(int layer, int x, int y)
    {
        if(m_GridRuntimeArray == null || !m_ArraySynced)
        {
            InitGridRuntimeArray();
        }
        if (x < m_minX || x > m_maxX || y < m_minY || y > m_maxY)
        {
            Debug.LogError("Coords out of Range");
            return false;
        }
            
        if (layer < m_LayerMin || layer > m_LayerMax)
        {
            Debug.LogError("Layer out of Range");
            return false;
        }
        //if(nr >= GridRuntimeArray(layer, x, y).Count)
        //{
        //    Debug.LogError("Nr out of Range");
        //    return false;
        //}

        return true;
    }

    private bool CheckValidLayer(int layerIndex)
    {
        if (PrefabList == null)
        {
            Debug.LogError("CheckValidLayer: prefablist not instantiated");
            return false;
        }
        if(PrefabList.GridLayers == null || PrefabList.GridLayers.Count == 0)
        {
            Debug.LogError("CheckValidLayer: PrefabList.GridLayers not instantiated");
            return false;
        }
        if (PrefabList.GridLayers.Where(l => l.LayerIndex == layerIndex).Count() == 0)
        {
            Debug.LogError("CheckValidLayer: layerIndex is not in the layerList");
            return false;
        }

        return true;    
    }

    /// <summary>
    ///  ######################### TODO #######################
    /// </summary>
    /// <param name="layer"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool Traversable(int layer, int x, int y){return false; }

    #endregion

    #region Awake

    public void Awake()
    {
        // Create RuntimeArray
        //InitGridRuntimeArray();
    }

    #endregion

    #region Init Array

    private void InitGridRuntimeArray()
    {
        Debug.Log("Initializing Runtime Grid Array");

        #region First find the size of the grid

        if (GridSaveData.Count > 0)
        {
            m_minX = int.MaxValue;
            m_minY = int.MaxValue;
            m_maxX = -int.MaxValue;
            m_maxY = -int.MaxValue;

            foreach (GridDataElement el in GridSaveData)
            {
                if (el.X < m_minX)
                    m_minX = el.X;
                if (el.Y < m_minY)
                    m_minY = el.Y;
                if (el.X > m_maxX)
                    m_maxX = el.X;
                if (el.Y > m_maxY)
                    m_maxY = el.Y;
            }
        }
        else
        {
            m_minX = 0;
            m_minY = 0;
            m_maxX = 0;
            m_maxY = 0;
        }

        #endregion

        #region Find the size of the layers
        if (PrefabList.GridLayers.Count > 0)
        {
            m_LayerMin = int.MaxValue;
            m_LayerMax = -int.MaxValue;

            foreach (GridLayer layer in PrefabList.GridLayers)
            {
                if (layer.LayerIndex < m_LayerMin)
                    m_LayerMin = layer.LayerIndex;
                if (layer.LayerIndex > m_LayerMax)
                    m_LayerMax = layer.LayerIndex;
            }
        }
        else
        {
            m_LayerMin = 0;
            m_LayerMax = 0;
        }

        #endregion


        Debug.Log(string.Format("x {0} {1} y {2} {3} layer {4} {5}",m_minX,m_maxX,m_minY,m_maxY,m_LayerMin,m_LayerMax));

        // Initialize array
        m_ArrayInitOnChanges = m_Changes;
        m_GridRuntimeArray = new List<GridDataElement>[Layers, GridWidth, GridHeight];

        foreach (GridDataElement el in GridSaveData)
        {
            if (GridRuntimeArray(el.Prefab.GridLayer, el.X, el.Y) == null)
                this[el.Prefab.GridLayer, el.X, el.Y] = new List<GridDataElement>();

            GridRuntimeArray(el.Prefab.GridLayer, el.X, el.Y).Add(el);
        }
    }

    

    #endregion

    #region Safe Add & Remove

    public bool SafeAdd(GridDataElement el, bool replace = true)
    {
        // Check if the layer allows it
        if (!CheckValidLayer(el.Prefab.GridLayer))
            return false;
            

        #region Layer,Coord check & Replace

        // If there is already something there on this layer
        bool alreadyObjectOnGridLayerCoord;// = true;
        //if(m_ArraySynced)
        //    alreadyObjectOnGridLayerCoord = GridRuntimeArray[el.Prefab.GridLayer, el.X, el.Y].Count() > 0;
        //else
            alreadyObjectOnGridLayerCoord = GridSaveData.Where(e => e.Prefab.GridLayer == el.Prefab.GridLayer && e.X == el.X && e.Y == el.Y).Count() > 0;

        if (alreadyObjectOnGridLayerCoord && !PrefabList.GridLayers[el.Prefab.GridLayer].AllowMultiplePerCoord)
        {
            if(replace)
            {
                ReplaceElement(el);
                Debug.Log("Element REPLACED @ " + el.X + " " + el.Y);
                return true;
            }
            Debug.Log("Element already exists not allowed to replace");
            return false;
        }

        #endregion

        Debug.Log("Element ADDED @ " + el.X + " " + el.Y);
        AddElementToSaveData(el);
        return true;
    }

    #region GridSaveData replace, add, remove

    private void AddElementToSaveData(GridDataElement el)
    {
        m_Changes++;
        GridSaveData.Add(el);
    }
    
    private bool RemoveElementFromSaveData(GridDataElement el)
    {
        if(GridSaveData.Contains(el))
        {
            m_Changes++;
            GridSaveData.Remove(el);
            el = null;
            return true;
        }
        return false;
    }

    private void ReplaceElement(GridDataElement el)
    {
        // Find old, Remove old, Add new
        GridDataElement old = GridSaveData.Where(e => e.Prefab.GridLayer == el.Prefab.GridLayer && e.X == el.X && e.Y == el.Y).First();
        DeleteAndRemoveFromData(old);
        AddElementToSaveData(el);
    }

    #endregion

    public GameObject SafeAddAndInstantiate(GridDataElement newElement, Grid grid)
    {
        if (!SafeAdd(newElement))
            return null;

        return FastIniatiate(newElement, grid);
    }

    public bool SafeRemove(GridDataElement element)
    {
        // Todo Checks
        if (!GridSaveData.Contains(element))
            return false;

        Debug.Log("Element REMOVED @ " + element.X + " " + element.Y);

        DeleteAndRemoveFromData(element);
        return true;
    }

    private void DeleteAndRemoveFromData(GridDataElement element)
    {
        GameObject.DestroyImmediate(element.Instance.gameObject);
        RemoveElementFromSaveData(element);
    }

    #endregion

    #region Instantiate

    private GameObject FastIniatiate(GridDataElement el, Grid grid)
    {
        Vector3 pos = grid.GetGridWorldPos(new Vector2(el.X,el.Y), el.Prefab.TileOffsetX, el.Prefab.TileOffsetY);
        GameObject go = GameObject.Instantiate(el.Prefab.Prefab, pos, Quaternion.identity) as GameObject;

        // Set to layerParent
        grid.CreateUpdateLayerContainers();
        GameObject layer = grid.LayerContainers.Where(c => c.Layer.LayerIndex == el.Prefab.GridLayer).First().GO;
        go.transform.parent = layer.transform;

        // Set GridElementInstance 
        el.Instance = go.GetComponent< GridElementInstance>();
        if (el.Instance == null)
            el.Instance = go.AddComponent<GridElementInstance>();

        el.Instance.ParentGrid = grid;
        el.Instance.Data = el;

        return go;
    }

    #endregion

    #region Instantiate All & Destroy All

    public void InstantiateAll(Grid grid)
    {
        foreach (GridDataElement el in GridSaveData)
        {
            FastIniatiate(el, grid);
        }
    }

    public void DeleteRuntime(Grid grid)
    {
        Debug.Log("DeleteRuntime");
        int amount = GridSaveData.Count;
        for (int i = amount - 1; i >= 0; i--)
        {
            GridDataElement el = GridSaveData[i];
            GameObject.DestroyImmediate(el.Instance.gameObject);
        }
    }

    public void DeleteAll(Grid grid)
    {
        Debug.Log("DeleteRuntime");
        int amount = GridSaveData.Count;
        for (int i = amount - 1; i >= 0; i--)
        {
            GridDataElement el = GridSaveData[i];
            DeleteAndRemoveFromData(el);
        }
        m_GridRuntimeArray = null;
        m_Changes = 0;
        m_ArrayInitOnChanges = -1;
    }

    #endregion
}
