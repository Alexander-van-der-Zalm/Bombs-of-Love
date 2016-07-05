using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Linq;

[CustomEditor(typeof(SpriteSorter))]
[CanEditMultipleObjects]
public class SpriteSorterEditor : Editor
{
    SpriteSorter sort;
    SpriteSorter[] sorts;

    void OnEnable()
    {
        sort = (SpriteSorter)target;
    }

    public override void OnInspectorGUI()
    {
        //sorts = targets as DepthSorter[];
        sorts = new SpriteSorter[targets.Length];
        for (int i = 0; i < targets.Length; i++)
            sorts[i] = (SpriteSorter)targets[i];

        base.OnInspectorGUI();
        if (GUILayout.Button("Sort"))
            SortNDraw();
    }

    private void SortNDraw()
    {
        sort.Sort();
        if(sorts.Length > 1)
        {
            foreach(SpriteSorter sor in sorts)
            {
                sor.Sort();
            }
        }
        
        SceneView.RepaintAll();
    }
}
