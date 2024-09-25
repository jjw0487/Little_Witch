using System;
using UnityEngine;

[Serializable]
public class PoolUnit<T>
{
    public Transform parent;
    public T unit;
    public int amount;
}
