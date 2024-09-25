using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataContainer",
    menuName = "ItemData/ItemDataContainer", order = 1)]
public class ItemDataContainer : Configurable
{
    [SerializeField] private ItemData[] datas;
    [SerializeField] private ItemObject[] objs;

    public ItemData GetItemData(int _id)
    {
        return datas[_id];
    }

    public ItemObject GetItemObject(int _id)
    {
        return objs[_id];
    }
}
