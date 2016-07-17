using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

[CustomPropertyDrawer(typeof(AxisKey))]
public class AxisKeyPD : PropertyDrawer
{
    KeyCodeEditorGUI m_KCNeg, m_KCPos;
    EnumEditorGUI<XboxAxis> m_XboxAxis;
    EnumEditorGUI<XboxDPad> m_XDpadNeg, m_XDpadPos;

    private void Init(SerializedProperty keys)
    {
        if (keys.arraySize == 0)
        {
            for (int i = 0; i < 2; i++)
            {
                keys.arraySize++;
                keys.GetArrayElementAtIndex(i).stringValue = "";
            }
        }

        if (m_KCNeg == null)
            m_KCNeg = new KeyCodeEditorGUI();
        if (m_KCPos == null)
            m_KCPos = new KeyCodeEditorGUI();

        if (m_XboxAxis == null)
            m_XboxAxis = new EnumEditorGUI<XboxAxis>();

        if (m_XDpadNeg == null)
            m_XDpadNeg = new EnumEditorGUI<XboxDPad>();
        if (m_XDpadPos == null)
            m_XDpadPos = new EnumEditorGUI<XboxDPad>();
    }

    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
    {
        SerializedProperty tp = prop.FindPropertyRelative("Type");
        AxisKey.AxisKeyType type = (AxisKey.AxisKeyType)Enum.Parse(typeof(AxisKey.AxisKeyType), tp.enumNames[tp.enumValueIndex], true);
        if(type == AxisKey.AxisKeyType.Axis)
            return EditorGUIUtility.singleLineHeight * 2;
        return EditorGUIUtility.singleLineHeight *3;
    }

    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        // Draw prefix label - if not a list item
        bool listItem = label.text.Contains("Element");
        if (!listItem)
            pos = EditorGUI.PrefixLabel(pos, GUIUtility.GetControlID(FocusType.Passive), label);

        // Get relevant properties
        SerializedProperty keys = prop.FindPropertyRelative("keys");
        SerializedProperty tp = prop.FindPropertyRelative("Type");
        SerializedProperty k0 = keys.GetArrayElementAtIndex(0);
        SerializedProperty k1 = keys.GetArrayElementAtIndex(1);

        Init(keys);

        var indent = EditorGUI.indentLevel;
        if(!listItem)
            EditorGUI.indentLevel = 0;
        EditorGUI.PropertyField(new Rect(pos.x,pos.y,pos.width,EditorGUIUtility.singleLineHeight), tp,GUIContent.none);
        if(!listItem)
            EditorGUI.indentLevel = indent;

        AxisKey.AxisKeyType type = (AxisKey.AxisKeyType)Enum.Parse(typeof(AxisKey.AxisKeyType), tp.enumNames[tp.enumValueIndex], true);

        
        pos.y += EditorGUIUtility.singleLineHeight;
        if (k0 != null)
        {
            if (type == AxisKey.AxisKeyType.Axis)
                m_XboxAxis.OnGUI(pos, k0, "", !listItem);
            else if (type == AxisKey.AxisKeyType.PC)
                m_KCPos.OnGUI(pos, k0, "+", !listItem);
            else
                m_XDpadPos.OnGUI(pos, k0, "+", !listItem);
        }
        pos.y += EditorGUIUtility.singleLineHeight;
        if (k0 != null)
        {
            if (type == AxisKey.AxisKeyType.PC)
                m_KCNeg.OnGUI(pos, k0, "-", !listItem);
            else if (type == AxisKey.AxisKeyType.Dpad)
                m_XDpadNeg.OnGUI(pos, k0, "-", !listItem);
        }
    }
}
