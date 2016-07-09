using UnityEngine;
using System.Collections;

[System.Serializable]
public class GridDataElement
{
    public GridPrefab Prefab;
    public GameObject Instance;
    public int X, Y;

    public bool Instantiated { get { return Instance != null; } }

    public GridDataElement(GridPrefab prefab, int x, int y)
    {
        Prefab = prefab;
        int X = x;
        int Y = y;
    }
}
