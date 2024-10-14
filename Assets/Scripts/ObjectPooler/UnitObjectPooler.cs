using System.Collections.Generic;
using UnityEngine;

public abstract class UnitObjectPooler<T>
{
    public UnitObjectPooler(T _prefabs, int _amount, Transform _parent)
    {
        prefab = _prefabs;
        amount = _amount;
        parent = _parent;
    }

    protected T prefab;
    protected List<T> pool;
    protected Transform parent;
    protected int amount;
    public virtual void CreatePool()
    {
        pool = new List<T>();

        for (int i = 0; i < amount; i++)
        {
            pool.Add(CreateObj());
        }
    }
    public abstract T GetObj();
    public abstract T CreateObj();
}
