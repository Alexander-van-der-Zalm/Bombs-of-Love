using UnityEngine;
using System.Collections;

public class BMGridElement : MonoBehaviour
{
    public enum GridType
    {
        Wall,
        Floor,
        Block
    }

    public GridType Type;
    public int x, y;
    public BMGrid ParentGrid;
}
