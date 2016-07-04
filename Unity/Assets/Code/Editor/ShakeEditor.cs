using UnityEngine;
using UnityEditor;
using NUnit.Framework;

[CustomEditor(typeof(Shake))]
public class ShakeEditor :Editor
{
    Shake shake;

    void OnEnable()
    {
        shake = (Shake)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("ShakeMyBooty"))
            shake.StartShake(shake.recipe,shake.transform);
    }
}
