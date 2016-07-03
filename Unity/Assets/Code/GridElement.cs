using UnityEngine;
using System.Collections;

public class GridElement : MonoBehaviour
{
    public enum GridType
    {
        Wall,
        Floor,
        Block
    }

    public GridType Type;
    public int x, y;
    public Grid ParentGrid;
}
