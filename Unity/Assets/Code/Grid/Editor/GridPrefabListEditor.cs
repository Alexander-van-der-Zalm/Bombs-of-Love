﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEditorInternal;
using System;

[CustomEditor(typeof(GridPrefabList))]
public class GridPrefabListEditor : Editor
{
    private ReorderableList prefabList;
    private ReorderableList layerList;

    private void OnEnable()
    {
        Debug.Log("Enable");
        InitializeLists();
    }

    public void InitializeLists()
    {
        prefabList = InitializePrefabList();
        layerList = InitializeLayerList();
    }

    private ReorderableList InitializeLayerList()
    {
        //public int Layer = 0;
        //public string Name = "Tile";
        //public int MaxInstancesPerCoord = 1;
        ReorderableList list = new ReorderableList(serializedObject,
                serializedObject.FindProperty("GridLayers"),
                true, true, true, true);

        int lW = 20;
        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, lW, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("Layer"), GUIContent.none);
            EditorGUI.PropertyField(
                new Rect(rect.x + 1 * lW, rect.y, rect.width -1 * lW, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("Name"), GUIContent.none);
            EditorGUI.PropertyField(
                new Rect(rect.x + rect.width - 1 * lW, rect.y, lW, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("MaxInstancesPerCoord"), GUIContent.none);
            
        };
        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "LayerList - Index - Name - MaxPerCoord");
        };
        list.onReorderCallback = (ReorderableList l) =>
        {
            SortLayerList(l);
        };
        list.onRemoveCallback = (ReorderableList l) =>
        {
            ReorderableList.defaultBehaviours.DoRemoveButton(l);
            SortLayerList(l);
        };
        list.onAddCallback = (ReorderableList l) =>
        {
            var index = l.serializedProperty.arraySize;
            l.serializedProperty.arraySize++;
            l.index = index;
            SortLayerList(l);
        };
        return list;
    }

    private void SortLayerList(ReorderableList l)
    {
        for (int i = 0; i < l.count; i++)
        {
            l.serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("Layer").intValue = i;
        }
    }

    public ReorderableList InitializePrefabList()
    {
        ReorderableList list = new ReorderableList(serializedObject,
                serializedObject.FindProperty("PrefabList"),
                true, true, true, true);

        int lW = 20;
        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, rect.width - 4 * lW, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("Prefab"), GUIContent.none);
            EditorGUI.PropertyField(
                new Rect(rect.x + rect.width - 4 * lW, rect.y, lW, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("GridLayer"), GUIContent.none);
            EditorGUI.PropertyField(
                new Rect(rect.x + rect.width - 3 * lW, rect.y, lW, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("TileOffsetX"), GUIContent.none);
            EditorGUI.PropertyField(
                new Rect(rect.x + rect.width - 2 * lW, rect.y, lW, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("TileOffsetY"), GUIContent.none);
            EditorGUI.PropertyField(
                new Rect(rect.x + rect.width - 1 * lW, rect.y, lW, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("Traversable"), GUIContent.none);
        };
        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Prefab List - layer - x,y offset - traversable");
        };
        list.onSelectCallback = (ReorderableList l) =>
        {
            var prefab = l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("Prefab").objectReferenceValue as GameObject;
            if (prefab)
                EditorGUIUtility.PingObject(prefab.gameObject);
        };
        return list;
    }

    //public GameObject Prefab;
    //public int GridLayer;
    //public float TileOffsetX;
    //public float TileOffsetY;
    //public bool Traversable;
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        layerList.DoLayoutList();
        prefabList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }

    //public override void OnInspectorGUI()
    //{
    //    base.OnInspectorGUI();
    //}

    public static void DrawGridPrefabList()
    {

    }
}