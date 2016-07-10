using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEditorInternal;
using System;

[CustomEditor(typeof(GridPrefabList))]
public class GridPrefabListEditor : Editor
{
    public GridPrefab SelectedGridPrefab;

    private ReorderableList prefabList;
    private ReorderableList layerList;
    private GridPrefabList gpl;

    private void OnEnable()
    {
        //Debug.Log("Enable");
        InitializeLists();
    }

    public void InitializeLists()
    {
        prefabList = InitializePrefabList(serializedObject);
        layerList = InitializeLayerList(serializedObject);
        gpl = target as GridPrefabList;
    }

    public ReorderableList InitializeLayerList(SerializedObject serializedObject)
    {
        
        //public int Layer = 0;
        //public string Name = "Tile";
        //public int MaxInstancesPerCoord = 1;
        ReorderableList list = new ReorderableList(serializedObject,
                serializedObject.FindProperty("GridLayers"),
                true, true, true, true);

        int lW1 = 20;
        int lW2 = 15;
        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, lW1, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("LayerIndex"), GUIContent.none);
            EditorGUI.PropertyField(
                new Rect(rect.x + 1 * lW1, rect.y, rect.width -1 * lW1 - 1 * lW2 , EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("Name"), GUIContent.none);
            EditorGUI.PropertyField(
                new Rect(rect.x + 2 + rect.width - 1 * lW2, rect.y, lW2, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("AllowMultiplePerCoord"), GUIContent.none);
            
        };
        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "LayerList - Index - Name - AllowMultiplePerCoord");
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
            l.serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("LayerIndex").intValue = i;
        }
    }

    public ReorderableList InitializePrefabList(SerializedObject serializedObject)
    {
        //public GameObject Prefab;
        //public int GridLayer;
        //public float TileOffsetX;
        //public float TileOffsetY;
        //public bool Traversable;

        ReorderableList list = new ReorderableList(serializedObject,
                serializedObject.FindProperty("PrefabList"),
                true, true, true, true);

        int lW1 = 20;
        int lW2 = 30;
        int lW3 = 15;
        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, rect.width - lW1 - 2 * lW2 - lW3, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("Prefab"), GUIContent.none);
            EditorGUI.PropertyField(
                new Rect(rect.x + rect.width - lW1 - 2 * lW2 - lW3, rect.y, lW1, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("GridLayer"), GUIContent.none);
            EditorGUI.PropertyField(
                new Rect(rect.x + 2 + rect.width - 2 * lW2 - lW3, rect.y, lW2, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("TileOffsetX"), GUIContent.none);
            EditorGUI.PropertyField(
                new Rect(rect.x + rect.width - lW2 - lW3, rect.y, lW2, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("TileOffsetY"), GUIContent.none);
            EditorGUI.PropertyField(
                new Rect(rect.x + rect.width - 1 * lW3 + 2, rect.y, lW3, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("Traversable"), GUIContent.none);
        };
        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Prefab List - layer - x,y offset - traversable");
        };
        list.onSelectCallback = (ReorderableList l) =>
        {
            //ReorderableList.defaultBehaviours.
            //var go = l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("Prefab").objectReferenceValue as GameObject;
            GridPrefabList gpl = serializedObject.targetObject as GridPrefabList;
            SelectedGridPrefab = gpl.PrefabList[l.index];
            //gpl.SelectedGridPrefab = gpl.PrefabList[l.index];
            //gpl.SelectedObject = gpl.SelectedGridPrefab.Prefab;

            if (SelectedGridPrefab.Prefab != null)
            {
                EditorGUIUtility.PingObject(SelectedGridPrefab.Prefab);
            }
        };

        return list;
    }


    public override void OnInspectorGUI()
    {
        //serializedObject.Update();
        layerList.DoLayoutList();
        prefabList.DoLayoutList();
        //serializedObject.ApplyModifiedProperties();
    }

    //public override void OnInspectorGUI()
    //{
    //    base.OnInspectorGUI();
    //}

    public static void DrawGridPrefabList()
    {

    }
}
