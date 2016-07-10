using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// This is all the data that describes a level
/// </summary>
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

    /// <summary>
    /// Lives Runtime
    /// </summary>
    [HideInInspector]
    public List<GridDataElement>[,,] GridRuntimeArray;

    private int m_minX, m_minY, m_maxX, m_maxY;
    private int m_LayerMin, m_LayerMax;

    [SerializeField,ReadOnly]
    private int m_Changes = 0;
    [SerializeField, ReadOnly]
    private int m_ArrayInitOnChanges = -1;

    private bool m_ArraySynced { get { return GridRuntimeArray != null && m_Changes == m_ArrayInitOnChanges; } }

    #endregion

    #region GetSet

    public GridDataElement this[int layer, int x, int y] { get { return this[layer, x, y, 0]; } }// Check out of bounds
    public GridDataElement this[int layer, int x, int y, int nr] { get { return CheckInArrayBounds(layer,x,y,nr) ? GridRuntimeArray[layer, x, y][nr] : null; } }
    public int Layers { get { return m_LayerMax - m_LayerMin + 1; } }
    public int GridWidth { get { return m_maxX - m_minX + 1; } }
    public int GridHeight { get { return m_maxY - m_minY + 1; } }

    private bool CheckInArrayBounds(int layer, int x, int y, int nr)
    {
        if(GridRuntimeArray == null)
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
        if(nr >= GridRuntimeArray[layer, x, y].Count)
        {
            Debug.LogError("Nr out of Range");
            return false;
        }

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

    public void InitGridRuntimeArray()
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
                if (layer.LayerIndex < m_LayerMax)
                    m_LayerMax = layer.LayerIndex;
            }
        }
        else
        {
            m_LayerMin = 0;
            m_LayerMax = 0;
        }
        #endregion

        // Initialize array
        m_ArrayInitOnChanges = m_Changes;
        GridRuntimeArray = new List<GridDataElement>[Layers, GridWidth, GridHeight];

        foreach (GridDataElement el in GridSaveData)
        {
            if (GridRuntimeArray[el.Prefab.GridLayer, el.X, el.Y] == null)
                GridRuntimeArray[el.Prefab.GridLayer, el.X, el.Y] = new List<GridDataElement>();

            GridRuntimeArray[el.Prefab.GridLayer, el.X, el.Y].Add(el);
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
        if(m_ArraySynced)
            alreadyObjectOnGridLayerCoord = GridRuntimeArray[el.Prefab.GridLayer, el.X, el.Y].Count() > 0;
        else
            alreadyObjectOnGridLayerCoord = GridSaveData.Where(e => e.Prefab.GridLayer == el.Prefab.GridLayer && e.X == el.X && e.Y == el.Y).Count() > 0;

        if (alreadyObjectOnGridLayerCoord && !PrefabList.GridLayers[el.Prefab.GridLayer].AllowMultiplePerCoord)
        {
            if(replace)
            {
                ReplaceElement(el);
                Debug.Log("Element safely replaced");
                return true;
            }
            Debug.Log("Element already exists not allowed to replace");
            return false;
        }

        #endregion

        Debug.Log("Element safely added");
        AddElement(el);
        return true;
    }

    #region GridSaveData replace, add, remove

    private void AddElement(GridDataElement el)
    {
        m_Changes++;
        GridSaveData.Add(el);
    }
    
    private bool RemoveElement(GridDataElement el)
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
        RemoveElement(old);
        AddElement(el);
    }

    #endregion

    public GameObject SafeAddAndInstantiate(GridDataElement newElement, Grid grid)
    {
        if (!SafeAdd(newElement))
            return null;

        return FastIniatiate(newElement, grid);
    }

    public void InstantiateAll(Grid grid)
    {
        throw new System.NotImplementedException();
    }

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

    public bool SafeRemove(GridDataElement element)
    {
        // Todo Checks
        //return false;

        return RemoveElement(element);
        //return true;
    }

    #endregion
}
