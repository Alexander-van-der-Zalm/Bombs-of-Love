using UnityEngine;
using UnityEditor;
using NUnit.Framework;

[CustomEditor(typeof(Grid))]
public class GridEditor : Editor
{
    private Grid grid;

    public void Awake()
    {
        Debug.Log("Awake");
        grid = (target as GameObject).GetComponent<Grid>();
    }

    public void OnSceneGUI()
    {
        // Refresh faster
        if (Event.current.type == EventType.MouseMove)
            SceneView.RepaintAll();

        // Keep this object selected
        int controlID = GUIUtility.GetControlID(FocusType.Native);
        if (Event.current.type == EventType.layout)
            HandleUtility.AddDefaultControl(controlID);

        // Spawn selecte object on mouseclick
        if (Event.current.type == EventType.MouseDown)
            Debug.Log("SpawnObject");
    }

    
}
