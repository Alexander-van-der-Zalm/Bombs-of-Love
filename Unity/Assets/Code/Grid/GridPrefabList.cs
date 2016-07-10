using UnityEngine;
using System.Collections.Generic;

public class GridPrefabList : ScriptableObject
{
    public List<GridLayer> GridLayers;
    public List<GridPrefab> PrefabList;
    //[HideInInspector]
    //public GameObject SelectedObject;
    //[HideInInspector]
    public GridPrefab SelectedGridPrefab { get { return PrefabList[SelectedIndex]; } }
    //[ReadOnly]
    public int SelectedIndex;
}

[System.Serializable]
public class GridLayer
{
    [ReadOnly]
    public int LayerIndex = 0;
    public string Name = "Tile";
    public bool AllowMultiplePerCoord = true;
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