using UnityEngine;
using XInputDotNetPure;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum ControlType
{
    PC,
    Xbox
}

// Needs custom inspector
[System.Serializable]
public class ControlKey 
{
    public ControlType Type;
    public string KeyValue;
    [HideInInspector]
    public bool LastState = false;
    [HideInInspector]
    public bool CurState = false;

    [SerializeField]
    private int selectedIndex;

    public ControlKey(ControlType type, string value)
    {
        Type = type;
        KeyValue = value;

        //Set selectedIndex
        if (Type == ControlType.PC)
            selectedIndex = Enum.GetNames(typeof(KeyCode)).ToList().FindIndex(e => e == KeyValue);
        else
            selectedIndex = Enum.GetNames(typeof(XboxButton)).ToList().FindIndex(e => e == KeyValue);
    }

    public static ControlKey XboxButton(XboxButton btn)
    {
        return new ControlKey(ControlType.Xbox, btn.ToString());
    }

    public static ControlKey PCKey(KeyCode kc)
    {
        return new ControlKey(ControlType.PC, kc.ToString());
    }

    #if UNITY_EDITOR

    public void OnGui()
    {
        GUILayout.Space(-20);
        Type = (ControlType)EditorGUILayout.EnumPopup(Type, GUILayout.Width(70.0f + 10 * EditorGUI.indentLevel));
        GUILayout.Space(-20);
        switch (Type)
        {
            case ControlType.PC:
                selectedIndex = EditorGUILayout.Popup(selectedIndex, ControlHelper.KeyCodeOptions, GUILayout.Width(80.0f + 10 * EditorGUI.indentLevel));
                if (selectedIndex >= ControlHelper.KeyCodeOptions.Length)
                    selectedIndex = 0;
                KeyValue = ControlHelper.KeyCodeOptions[selectedIndex];
                break;

            case ControlType.Xbox:
                selectedIndex = EditorGUILayout.Popup(selectedIndex, ControlHelper.XboxButtonOptions, GUILayout.Width(60.0f + 10 * EditorGUI.indentLevel));
                if (selectedIndex >= ControlHelper.XboxButtonOptions.Length)
                    selectedIndex = 0;
                KeyValue = ControlHelper.XboxButtonOptions[selectedIndex];
                break;
        }
    }

    private void pcGui()
    {
        selectedIndex = EditorGUILayout.Popup(selectedIndex, ControlHelper.XboxAxixOptions, GUILayout.Width(80.0f));
    }

    #endif
}
