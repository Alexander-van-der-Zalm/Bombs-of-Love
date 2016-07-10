using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEditor;

public class Grid : MonoBehaviour
{
    #region Fields

    public GridData LevelData;

    public float TileWidth { get { return LevelData != null ? LevelData.TileWidth : 1.0f; } }
    public float TileHeight { get { return LevelData != null ? LevelData.TileHeight : 1.0f; } }

    public KeyCode SpawnObjectKeyCode = KeyCode.A;
    //public bool DrawLinesInGame = true; // IMPLEMENT PLX

    public GridLineDrawer drawer;

    public List<GridLayerContainer> LayerContainers;

    [ReadOnly]
    public Vector2 SelectedGridCoord;
    [ReadOnly]
    public GameObject ObjectToSpawn;
    [HideInInspector]
    public GridPrefab SelectedGridPrefab;

    private Vector2 m_LastSpawnedGridCoord;
    private GameObject m_LastObjectSpawned;

    #endregion

    public void Update()
    {
        Debug.Log("Update");
        if (SelectedGridCoord != m_LastSpawnedGridCoord && Input.GetKey(SpawnObjectKeyCode))
        {
            Debug.Log("SpawnObject @ " + SelectedGridCoord);
            m_LastSpawnedGridCoord = SelectedGridCoord;
        }

    }

    public void SpawnObject()
    {
        if (SelectedGridCoord == m_LastSpawnedGridCoord && ObjectToSpawn == m_LastObjectSpawned)
            return;

        Vector3 pos = GetGridWorldPos(SelectedGridCoord);

        Debug.Log("SpawnObject: " + ObjectToSpawn.name + " @ " + SelectedGridCoord + " - " + pos);
        // Move Instantiating to gridData?
        GridDataElement el = new GridDataElement(SelectedGridPrefab, (int)SelectedGridCoord.x, (int)SelectedGridCoord.y);

        // Check GridElement on prefabs...
        
        //el.Instance = GameObject.Instantiate(ObjectToSpawn, pos, Quaternion.identity) as GameObject;

        LevelData.SafeAddAndInstantiate(el,this);
        //Debug.Log("Register to leveldata plz");

        m_LastObjectSpawned = ObjectToSpawn;
        m_LastSpawnedGridCoord = SelectedGridCoord;
    }

    #region Create/Update Layer Containers

    public void CreateUpdateLayerContainers()
    {
        if (LayerContainers == null)
            LayerContainers = new List<GridLayerContainer>();

        // Check if it needs a new container
        foreach(GridLayer layer in LevelData.PrefabList.GridLayers)
        {
            GridLayerContainer container = LayerContainers.Where(lc => lc.Layer == layer).FirstOrDefault();
            Debug.Log(layer.Name + " hash " + layer.Hash);
            if (layer.Hash < 0)
                layer.Hash = layer.GetHashCode();
            
            // Check if reference is lost
            if (container == null)
            {
                container = LayerContainers.Where(lc => lc.Layer.Hash == layer.Hash).FirstOrDefault();
                if (container != null)
                    container.Layer = layer;
            }

            // If container with empty go
            if (container != null && container.GO == null) 
            {
                LayerContainers.Remove(container);
                container = null;
            }

            // If no container yet 
            if (container == null)
            {
                GameObject go = new GameObject(layer.Name);
                go.transform.parent = this.transform;
                container = new GridLayerContainer() { Layer = layer, GO = go };
                LayerContainers.Add(container);
            }
        }
        // Update all containers
        int containerAmount = LayerContainers.Count;
        for(int i = containerAmount-1; i > -1; i--)
        { 
            GridLayerContainer container = LayerContainers[i]; 
                // Remove if there is no more gameObject
            if (container.GO == null)
            {
                LayerContainers.Remove(container);  
            }
            else
            {
                container.GO.name = container.Layer.Name;
                container.GO.transform.SetSiblingIndex(container.Layer.LayerIndex);
            }
        }
        AssetDatabase.SaveAssets(); 
    }

    #endregion

    #region Gizmos

    public void OnDrawGizmos()
    {
        if (drawer.DrawLinesInEditor)
        {
            // Draw grid
            Vector3 pos = GetGridSnappedPos(Camera.current.transform.position);
            drawer.DrawGridInAllDirections(1000, 500, TileWidth, TileWidth, pos, drawer.GridColor); // potentially change to the size of the camerawidth
        }
    }

    public void OnDrawGizmosSelected()
    {
        if (drawer.DrawLinesInEditor)
        {
            // Draw hover
            Vector3 pos = GetGridSnappedPos(UnityEditor.HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin);
            drawer.DrawGrid(1, 1, TileWidth, TileWidth, pos, drawer.HoverColor);

            // Draw Selected
            pos = GetGridSnappedPos(UnityEditor.HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin);
            drawer.DrawGrid(1, 1, TileWidth, TileWidth, pos, drawer.SelectedColor);
        }
    }

    #endregion

    #region Get

    public Vector2 GetSceneMouseGridCoord()
    {
        return GetGridCoord(UnityEditor.HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin);
    }

    public Vector2 GetGridCoord(Vector3 worldPos)
    {
        return new Vector2(Mathf.Floor(worldPos.x / TileWidth), Mathf.Floor(worldPos.y / TileHeight));
    }

    public Vector3 GetGridSnappedPos(Vector3 worldPos, float tileXOffset = 0, float tileYOffset = 0)
    {
        return GetGridWorldPos(GetGridCoord(worldPos)) + new Vector3(tileXOffset * TileWidth, tileYOffset * TileHeight);
    }

    public Vector3 GetGridWorldPos(Vector2 gridCoord)
    {
        return new Vector3(gridCoord.x * TileWidth, gridCoord.y * TileHeight);
    }

    #endregion
}

[System.Serializable]
public class GridLayerContainer
{
    public GameObject GO;
    public GridLayer Layer;
}

#region Grid Drawer

[System.Serializable]
public class GridLineDrawer
{
    public bool DrawLinesInEditor = true;
    public Color GridColor = Color.gray;
    public Color SelectedColor = Color.green;
    public Color HoverColor = Color.blue;

    static Material lineMaterial;
    static void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }

    internal void DrawGrid(int gridWidth, int gridHeight, float tileWidth, float tileHeight, Vector3 offset, Color lineColor)
    {
        CreateLineMaterial();
        // Apply the line material
        lineMaterial.SetPass(0);

        GL.Begin(GL.LINES);
        GL.Color(lineColor);
        int x = 0;
        int y = 0;
        for (y = 0; y <= gridHeight; y++)
        {
            GL.Vertex(new Vector3(0, y * tileHeight, 0) + offset);
            GL.Vertex(new Vector3(tileWidth * gridWidth, y * tileHeight, 0) + offset);
        }
        for (x = 0; x <= gridWidth; x++)
        {
            GL.Vertex(new Vector3(x * tileWidth, 0, 0) + offset);
            GL.Vertex(new Vector3(x * tileWidth, tileHeight * gridHeight, 0) + offset);
        }
        GL.End();

    }

    internal void DrawGridInAllDirections(int gridWidth, int gridHeight, float tileWidth, float tileHeight, Vector3 offset, Color lineColor)
    {
        CreateLineMaterial();
        // Apply the line material
        lineMaterial.SetPass(0);

        GL.Begin(GL.LINES);
        GL.Color(lineColor);
        int x = 0;
        int y = 0;
        // TopRight
        for (y = 0; y <= gridHeight; y++)
        {
            GL.Vertex(new Vector3(0, y * tileHeight, 0) + offset);
            GL.Vertex(new Vector3(tileWidth * gridWidth, y * tileHeight, 0) + offset);
        }
        for (x = 0; x <= gridWidth; x++)
        {
            GL.Vertex(new Vector3(x * tileWidth, 0, 0) + offset);
            GL.Vertex(new Vector3(x * tileWidth, tileHeight * gridHeight, 0) + offset);
        }
        // TopRight
        for (y = 0; y <= gridHeight; y++)
        {
            GL.Vertex(new Vector3(0, y * tileHeight, 0) + offset);
            GL.Vertex(new Vector3(-tileWidth * gridWidth, y * tileHeight, 0) + offset);
        }
        for (x = 0; x <= gridWidth; x++)
        {
            GL.Vertex(new Vector3(-x * tileWidth, 0, 0) + offset);
            GL.Vertex(new Vector3(-x * tileWidth, tileHeight * gridHeight, 0) + offset);
        }
        // TopRight
        for (y = 0; y <= gridHeight; y++)
        {
            GL.Vertex(new Vector3(0, -y * tileHeight, 0) + offset);
            GL.Vertex(new Vector3(tileWidth * gridWidth, -y * tileHeight, 0) + offset);
        }
        for (x = 0; x <= gridWidth; x++)
        {
            GL.Vertex(new Vector3(x * tileWidth, 0, 0) + offset);
            GL.Vertex(new Vector3(x * tileWidth, -tileHeight * gridHeight, 0) + offset);
        }
        // TopRight
        for (y = 0; y <= gridHeight; y++)
        {
            GL.Vertex(new Vector3(0, -y * tileHeight, 0) + offset);
            GL.Vertex(new Vector3(-tileWidth * gridWidth, -y * tileHeight, 0) + offset);
        }
        for (x = 0; x <= gridWidth; x++)
        {
            GL.Vertex(new Vector3(-x * tileWidth, 0, 0) + offset);
            GL.Vertex(new Vector3(-x * tileWidth, -tileHeight * gridHeight, 0) + offset);
        }
        GL.End();

    }
}

#endregion
