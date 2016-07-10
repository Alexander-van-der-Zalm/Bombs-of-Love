using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class GridPrefabList : ScriptableObject
{
    public List<GridLayer> GridLayers;
    public List<GridPrefab> PrefabList;

    public GridPrefab SelectedGridPrefab { get { return PrefabList[(int)Mathf.Max(0,SelectedPrefabIndex)]; } }
    [ReadOnly]
    public int SelectedPrefabIndex;
    [ReadOnly]
    public int SelectedLayerIndex;
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