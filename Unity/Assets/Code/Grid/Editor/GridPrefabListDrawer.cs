using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(GridPrefabList))]
public class GridPrefabListDrawer : PropertyDrawer
{
    private ReorderableList prefabList;
    private ReorderableList layerList;
    private SerializedObject so;
    private GridPrefabList gpl;

    private int sy = 5;
    //[SerializeField]
    //private int prefabIndex = -1;
    //[SerializeField]
    //private int layerIndex = -1;
    //private GridPrefabListEditor editor;

    private void Init(SerializedProperty prop)
    {
        if (so == null)
            so = new SerializedObject(prop.objectReferenceValue);

        if (gpl == null)
            gpl = prop.objectReferenceValue as GridPrefabList;

        //if(editor == null)
        //    editor = Editor.CreateEditor(prop.objectReferenceValue) as GridPrefabListEditor;

        if (prefabList == null)
            prefabList = InitializePrefabList(so);
            
        if (layerList == null)
            layerList = InitializeLayerList(so);
    }

    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
    {
        Init(prop);

        return prefabList.GetHeight() + layerList.GetHeight() + 3* EditorGUIUtility.singleLineHeight + sy * 2;
    }

    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        Init(prop);
        //so = new SerializedObject(prop.objectReferenceValue);
        //so.Update();
        EditorGUI.BeginChangeCheck();
        
        EditorGUI.PropertyField(new Rect(pos.x, pos.y + sy, pos.width, EditorGUIUtility.singleLineHeight), prop);
        layerList.DoList((new Rect(pos.x, pos.y + sy * 2 + EditorGUIUtility.singleLineHeight, pos.width, layerList.GetHeight())));
        prefabList.DoList(new Rect(pos.x, pos.y + sy * 2 + EditorGUIUtility.singleLineHeight + layerList.GetHeight(), pos.width, prefabList.GetHeight()));
        EditorGUI.PropertyField(new Rect(pos.x, pos.y + sy * 2 + EditorGUIUtility.singleLineHeight + layerList.GetHeight() + prefabList.GetHeight(), pos.width, EditorGUIUtility.singleLineHeight), so.FindProperty("SelectedPrefabIndex"));
        EditorGUI.PropertyField(new Rect(pos.x, pos.y + sy * 2 + 2 * EditorGUIUtility.singleLineHeight + layerList.GetHeight() + prefabList.GetHeight(), pos.width, EditorGUIUtility.singleLineHeight), so.FindProperty("SelectedLayerIndex"));

        if (EditorGUI.EndChangeCheck())
            so.ApplyModifiedProperties();

        //if(prefabIndex >= 0)
        //    prefabIndex
        
        //prefabList.
        prefabList.index = gpl.SelectedPrefabIndex;
        layerList.index = gpl.SelectedLayerIndex;
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
                new Rect(rect.x + 1 * lW1, rect.y, rect.width - 1 * lW1 - 1 * lW2, EditorGUIUtility.singleLineHeight),
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
            l.serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("Hash").intValue = -1;
            SortLayerList(l);
        };
        list.onSelectCallback = (ReorderableList l) =>
        {
            GridPrefabList gpl = serializedObject.targetObject as GridPrefabList;
            gpl.SelectedLayerIndex = l.index;
            gpl.SelectedPrefabIndex = -1;
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
            GridPrefabList gpl = serializedObject.targetObject as GridPrefabList;
            gpl.SelectedPrefabIndex = l.index;
            gpl.SelectedLayerIndex = gpl.PrefabList[l.index].GridLayer;
            if (gpl.PrefabList[l.index].Prefab != null)
            {
                EditorGUIUtility.PingObject(gpl.PrefabList[l.index].Prefab);
            }
        };

        return list;
    }
}

        //if (gpl == null)
        //    gpl = fieldInfo.GetValue(prop.serializedObject.targetObject) as GridPrefabList;
//public class GridPrefabList : ScriptableObject
//{
//    public List<GridLayer> GridLayers;
//    public List<GridPrefab> PrefabList;
//}

//[System.Serializable]
//public class GridLayer
//{
//    [ReadOnly]
//    public int Layer = 0;
//    public string Name = "Tile";
//    public bool AllowMultiplePerCoord = true;
//}

//[System.Serializable]
//public class GridPrefab
//{
//    public GameObject Prefab;
//    public int GridLayer;
//    public float TileOffsetX;
//    public float TileOffsetY;
//    public bool Traversable;
//}