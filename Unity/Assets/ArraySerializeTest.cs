using UnityEngine;
using System.Collections.Generic;


public class ArraySerializeTest : MonoBehaviour {

    public List<List<int>> listTest;
    public List<IntList> l2;
   // public UDictionaryStringBool dic;
}

[System.Serializable]
public class IntList
{
    public List<int> L;
    public D2 L2;
}

[System.Serializable]
public class D2
{
    public List<int> L;
    public List2D<int> L2;
}

[System.Serializable]
public class List2D<T> : List<T>
{
    public List2D()
    {
    }

    public List2D(int capacity) : base(capacity)
    {
    }

    public List2D(IEnumerable<T> collection) : base(collection)
    {
    }
}
