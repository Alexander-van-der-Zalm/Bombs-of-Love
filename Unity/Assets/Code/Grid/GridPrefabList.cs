using UnityEngine;
using System.Collections.Generic;

public class GridPrefabList : ScriptableObject
{
    public List<GridLayer> GridLayers;
    public List<GridPrefab> PrefabList;

    public GridPrefab SelectedGridPrefab { get { return PrefabList[SelectedIndex]; } }
    [ReadOnly]
    public int SelectedIndex;
}

[System.Serializable]
public class GridLayer
{
    [ReadOnly]
    public int LayerIndex = 0;
    public string Name = "Tile";
    public bool AllowMultiplePerCoord = true;
    [ReadOnly]
    public int Hash = -1;

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