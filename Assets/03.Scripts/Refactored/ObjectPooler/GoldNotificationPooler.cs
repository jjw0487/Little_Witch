using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldNotificationPooler : UnitObjectPooler<GoldAcquiredNotification>
{
    public GoldNotificationPooler(GoldAcquiredNotification _prefab, int _initAmount,
        Transform _parent)
        : base(_prefab, _initAmount, _parent)
    {
        CreatePool();
    }

    public override GoldAcquiredNotification CreateObj()
    {
        var obj = Object.Instantiate(prefab);
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(parent);
        obj.name = "GoldNotification";
        return obj;
    }

    public override GoldAcquiredNotification GetObj()
    {

        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].IsOn())
            {
                return pool[i];
            }
        }

        var obj = CreateObj();

        pool.Add(obj);

        return obj;
    }
}
