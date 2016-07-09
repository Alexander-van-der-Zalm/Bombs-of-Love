using UnityEngine;
using System.Collections.Generic;

public class GridPrefabList : ScriptableObject
{
    public List<GridLayer> GridLayers;
    public List<GridPrefab> PrefabList;
}

[System.Serializable]
public class GridLayer
{
    public int Layer = 0;
    public string Name = "Tile";
    public int MaxInstancesPerCoord = 1;
}

[System.Serializable]
public class GridPrefab
{
    public GameObject Prefab;
    public int GridLayer;
    public float TileOffsetX;
    public float TileOffsetY;
    public bool Traversable;
}