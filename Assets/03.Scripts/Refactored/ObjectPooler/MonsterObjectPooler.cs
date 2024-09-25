using System;
using UnityEngine;

public class MonsterObjectPooler : UnitObjectPooler<Monster>
{
    public MonsterObjectPooler(Monster prefab, int initAmount,
       Transform parent, Action _deathCountCallback)
       : base(prefab, initAmount, parent)
    {
        deathCountCallback = _deathCountCallback;
        CreatePool();
    }

    private Action deathCountCallback;

    public override Monster CreateObj()
    {
        var obj = GameObject.Instantiate(prefab);
        obj.Initialize(deathCountCallback);
        obj.transform.parent = parent;
        obj.gameObject.SetActive(false);
        return obj;
    }

    public override Monster GetObj()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].IsAlive())
            {
                return pool[i];
            }
        }
        var obj = CreateObj();
        pool.Add(obj);
        return obj;
    }
}
