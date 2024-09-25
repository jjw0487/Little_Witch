using System.Collections.Generic;
using UnityEngine;

public class ItemObjectPooler : MonoBehaviour
{
    private Dictionary<int, List<ItemObject>> pool = new Dictionary<int, List<ItemObject>>();

    private void OnEnable()
    {
        EventManager.itemSpawnEvent += SpawnItem;
    }
    private void OnDisable()
    {
        EventManager.itemSpawnEvent -= SpawnItem;
    }
    private void SpawnItem(Vector3 pos, int value, ItemObject item)
    {
        if (item.IsPoolable()) SpawnPoolableItem(pos, value, item);
        else SpawnDisposableItem(pos, value, item);
    }

    private void SpawnPoolableItem(Vector3 pos, int value, ItemObject item)
    {
        if (pool.TryGetValue(item.GetItemId(), out List<ItemObject> list))
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (!list[i].IsSpawned())
                {
                    list[i].Spawn(pos, value);
                    return;
                }
            }

            // 리스트 안에 내용물이 모두 사용중일 경우 새로 생성
            var obj = CreateItemObject(pos, value, item);
            list.Add(obj);
        }
        else
        {

            var obj = CreateItemObject(pos, value, item);

            List<ItemObject> itemList = new List<ItemObject>();

            itemList.Add(obj);

            pool.Add(item.GetItemId(), itemList);

        }
    }

    private void SpawnDisposableItem(Vector3 pos, int value, ItemObject item)
    {
        CreateItemObject(pos, value, item);
    }

    private ItemObject CreateItemObject(Vector3 pos, int value, ItemObject item)
    {
        var obj = Instantiate(item, pos, Quaternion.identity, this.transform);

        obj.Spawn(pos, value);

        return obj;
    }

}
