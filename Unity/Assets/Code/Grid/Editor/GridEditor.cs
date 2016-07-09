﻿using UnityEngine;
using UnityEditor;
using NUnit.Framework;

[CustomEditor(typeof(Grid))]
public class GridEditor : Editor
{
    private Grid grid;
    private bool SpawnKeyDown = false;

    public void Awake()
    {
        Debug.Log("Awake");
        grid = Selection.activeGameObject.GetComponent<Grid>();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    public void OnSceneGUI()
    {
        if (grid == null)
            grid = Selection.activeGameObject.GetComponent<Grid>();
        //Debug.Log("OnSceneGUI");
        // Refresh faster
        if (Event.current.type == EventType.MouseMove)
        {
            SceneView.RepaintAll();
            if(grid != null)
                grid.SelectedGridCoord = grid.GetSceneMouseGridCoord();
        }

        if(Event.current.keyCode == grid.SpawnObjectKeyCode)
        {
            //Debug.Log("KeyCode: " + Event.current.keyCode);
            grid.SpawnObject();
            SceneView.RepaintAll();
        }

        grid.ObjectToSpawn = grid.LevelData.PrefabList.SelectedObject;
        // Keep this object selected
        int controlID = GUIUtility.GetControlID(FocusType.Native);
        if (Event.current.type == EventType.layout)
            HandleUtility.AddDefaultControl(controlID);

        //// Spawn selecte object on mouseclick
        if (Event.current.type == EventType.MouseUp)
        {
            grid.SpawnObject();
            SceneView.RepaintAll();
        }
    }

    
}
