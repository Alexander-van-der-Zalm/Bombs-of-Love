using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;

[CustomPropertyDrawer(typeof(Axis))]
public class AxisPD : PropertyDrawer
{
    [SerializeField]
    private ReorderableList list;
    [SerializeField]
    private string headerLabel;

    private void Init(SerializedProperty prop, GUIContent label)
    {
        if (list == null)
        {
            headerLabel = label.text; // Save right label

            list = new ReorderableList(prop.serializedObject, prop.FindPropertyRelative("AxisKeys"), true, true, true, true);
            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = list.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(rect, element);
            };
            list.drawHeaderCallback = (Rect rect) =>
            {
                Debug.Log(label.text.ToString());
                EditorGUI.LabelField(rect, "Axis - " + headerLabel);
            };
            //list.elementHeight = EditorGUIUtility.singleLineHeight * 2 + 2;
            list.elementHeightCallback = (index) =>
             {
                 var element = list.serializedProperty.GetArrayElementAtIndex(index);
                 return EditorGUI.GetPropertyHeight(element) + 2;
             };
        }
    }

    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
    {
        Init(prop, label);

        return EditorGUIUtility.singleLineHeight *1 + list.GetHeight();
    }

    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        Init(prop, label);
        Debug.Log(label.text.ToString());
        list.DoList(pos);
    }
}
