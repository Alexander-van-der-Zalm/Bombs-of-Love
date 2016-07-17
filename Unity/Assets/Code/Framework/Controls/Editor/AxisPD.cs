using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;

[CustomPropertyDrawer(typeof(Axis))]
public class AxisPD : PropertyDrawer
{
    private ReorderableList list;

    private void Init(SerializedProperty prop, GUIContent label)
    {
        if (list == null)
        {
            list = new ReorderableList(prop.serializedObject, prop.FindPropertyRelative("AxisKeys"), true, true, true, true);
            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = list.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(rect, element);
                //Debug.Log(element.propertyPath);
            };
            list.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, "Axis - " + label.text);
            };
            list.elementHeight = EditorGUIUtility.singleLineHeight * 3 + 2;
        }
    }

    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
    {
        Init(prop, label);

        return EditorGUIUtility.singleLineHeight *6 + list.GetHeight();
    }

    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        Init(prop, label);
        list.DoList(pos);
    }
}
