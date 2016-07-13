using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

[CustomPropertyDrawer(typeof(ActionKey))]
public class ActionKeyPD : PropertyDrawer
{
    private int m_TypeIndex = 0;
    private int m_ValueIndex = 0;

    [SerializeField]
    private XboxButton m_XboxEnum;
    [SerializeField]
    private KeyCode m_KeyCode;
    [SerializeField]
    private string IncompleteKeyCode;
    [SerializeField]
    private string lastValidKeyCode;

    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        EditorGUI.BeginProperty(pos, GUIContent.none, prop);

        SerializedProperty tp = prop.FindPropertyRelative("Type");
        SerializedProperty kp = prop.FindPropertyRelative("KeyValue");
        
        //PrivateVarsCheck()


        // Prefix Label
        pos = EditorGUI.PrefixLabel(pos, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        Debug.Log(EditorGUI.indentLevel);
        EditorGUI.indentLevel = 0;

        float splitWidth = (pos.width - 45 - 3)/3;

        // Rects
        Rect typeRect       = new Rect(pos.x                            , pos.y, 45, EditorGUIUtility.singleLineHeight);
        Rect valueRect      = new Rect(pos.x + 45 + 2                   , pos.y, splitWidth, EditorGUIUtility.singleLineHeight);
        Rect textRect       = new Rect(pos.x + 45 + 3 + splitWidth      , pos.y, splitWidth, EditorGUIUtility.singleLineHeight);
        Rect enumRect       = new Rect(pos.x + 45 + 4 + 2 * splitWidth  , pos.y, splitWidth, EditorGUIUtility.singleLineHeight);
        Rect xboxEnumRext   = new Rect(pos.x + 45 + 3 + splitWidth      , pos.y, 2 * splitWidth, EditorGUIUtility.singleLineHeight);

        // Draw
        EditorGUI.PropertyField(typeRect, tp, GUIContent.none);
        ControlType type = (ControlType)Enum.Parse(typeof(ControlType), tp.enumNames[tp.enumValueIndex], true);
        //switch(tp.En)
        if (type == ControlType.PC)
        {
            EditorGUI.BeginChangeCheck();
            m_KeyCode = (KeyCode)EditorGUI.EnumPopup(enumRect, m_KeyCode);
            if (EditorGUI.EndChangeCheck())
                SetKeyCodeValueProperty(kp, m_KeyCode.ToString());

            EditorGUI.BeginChangeCheck();
            IncompleteKeyCode = EditorGUI.TextField(textRect, IncompleteKeyCode);

            // Check if it is a valid input
            if (EditorGUI.EndChangeCheck())
            {
                if(!Enum.IsDefined(typeof(KeyCode), IncompleteKeyCode))
                {
                    Debug.Log("Not correct");
                }
                else
                {
                    SetKeyCodeValueProperty(kp, IncompleteKeyCode);
                    m_KeyCode = ControlHelper.ReturnKeyCode(IncompleteKeyCode);
                }
            }
        }
        else
        {
            EditorGUI.BeginChangeCheck();
            m_XboxEnum = (XboxButton)EditorGUI.EnumPopup(xboxEnumRext, m_XboxEnum);
            if (EditorGUI.EndChangeCheck())
                SetKeyCodeValueProperty(kp, m_XboxEnum.ToString());
        }
        EditorGUI.PropertyField(valueRect, kp, GUIContent.none);
        // Set indent back to what it was
        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();

    }

    private void SetKeyCodeValueProperty(SerializedProperty kp, string value)
    {
        kp.stringValue = value;
        lastValidKeyCode = value;
    }
}
