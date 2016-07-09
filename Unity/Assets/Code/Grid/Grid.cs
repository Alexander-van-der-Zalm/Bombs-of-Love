using UnityEngine;
using System.Collections;
using System;

//[ExecuteInEditMode]
public class Grid : MonoBehaviour
{
    #region Fields

    public float TileWidth = 1.0f;
    public float TileHeight = 1.0f;

    
    //public bool DrawLinesInGame = true; // IMPLEMENT PLX

    #endregion

    public GridLineDrawer drawer;
    public Vector2 SelectedGridCoord;

    //public void Update()
    //{
    //    if (Event.current.type == EventType.MouseMove)
    //        SceneView.RepaintAll();
    //}

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
