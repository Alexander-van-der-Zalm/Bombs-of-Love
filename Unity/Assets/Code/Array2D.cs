using UnityEngine;
using System.Collections;

[System.Serializable]
public class Array2D<T>
{
    public T this[int x, int y]
    {
        get { return array1D[width * y + x]; }
        set { array1D[width * y + x] = value; }
    }

    public int Width { get { return width; } }
    public int Height { get { return height; } }

    [SerializeField]
    private T[] array1D;
    [SerializeField]
    private int width = 0;
    [SerializeField]
    private int height = 0;

    public Array2D(int width, int height)
    {
        this.width = width;
        this.height = height;
    }
}
