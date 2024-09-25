using Enums;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Collected
{ // 중복 아이템이 인벤토리에 분할되어 있을 경우를 대비해 수량을 통합 계산하기 위함 
    public int value;
    public Collected(int _value)
    {
        value = _value;
    }
}

public class InventoryData
{
    public InventoryData(ItemDataContainer _dataContainer)
    {
        dataContainer = _dataContainer;

        LoadSavedData();
    }

    private ItemDataContainer dataContainer;
    public Dictionary<int, Collected> possession = new Dictionary<int, Collected>();
    public ItemSlotData[] inventorySlot = new ItemSlotData[25];
    public ItemSlotData[] equipmentSlot = new ItemSlotData[4];

    private int gold;
    public int Gold
    {
        get => gold;
        set
        {
            if(EventManager.goldNotificationEvent != null)
            {
                EventManager.goldNotificationEvent(value);
            }
            
            gold = gold + value;
            PSave.Save("Gold", gold);
        }
    }
    public ItemData GetItemData(int id) => dataContainer.GetItemData(id);
    public ItemObject GetItemObject(int id) => dataContainer.GetItemObject(id);
    private void LoadSavedData()
    {
        // 게임 재화
        Gold = PLoad.Load("Gold", 200); // 게임 초회 실행시 200원

        // 인벤토리 슬롯
        for (int i = 0; i < inventorySlot.Length; i++)
        {
            string s = PLoad.Load($"Slot_{i}", "");

            if (string.IsNullOrEmpty(s))
            {
                inventorySlot[i] = new ItemSlotData(Enums.SlotType.Item, i);
            }
            else
            {
                int[] arr =  ConvertTo.StringToIntArr(s);

                if (arr[0] == 100)
                {
                    inventorySlot[i] = new ItemSlotData(Enums.SlotType.Item, i);
                    continue;
                }

                ItemData data = dataContainer.GetItemData(arr[0]);

                inventorySlot[i] = new ItemSlotData(Enums.SlotType.Item, i, data, arr[1]);

                if (possession.TryGetValue(arr[0], out Collected col))
                {
                    col.value += arr[1];
                }
                else
                {
                    possession.Add(arr[0], new Collected(arr[1]));
                }
            }
        }
        // 장비는 딕셔너리에 담지 않음.
        // 아이템끼리 겹쳐지도록 처리하지 않기 때문 ( 1개에 한 칸 차지 )
        for(int i = 0; i < equipmentSlot.Length; i++)
        {
            int equip = i + 25;

            string s = PLoad.Load($"Slot_{equip}", "");

            if (string.IsNullOrEmpty(s))
            {
                equipmentSlot[i] = new ItemSlotData(Enums.SlotType.Equipment, equip);
            }
            else
            {
                int[] arr = ConvertTo.StringToIntArr(s);

                if (arr[0] == 100)
                {
                    inventorySlot[i] = new ItemSlotData(Enums.SlotType.Equipment, i);
                    continue;
                }

                ItemData data = dataContainer.GetItemData(arr[0]);

                equipmentSlot[i] = new ItemSlotData(Enums.SlotType.Equipment, equip, data, arr[1]);
            }
        }
    }
    public bool Acquire(ItemData data, int _value)
    {
        // false를 리턴하는 상황들
        // => 인벤토리가 꽉 찼을 경우.
        // (장비는 수량이 추가되지 않고 한 개가 한 칸을 차지함.)

        ItemType type = data.Type;

        int _itemId = data.ItemId;

        if (type == Enums.ItemType.Equipment)
        {
            if (FindEmptySlot(_itemId, _value))
            {
                // 장비는 딕셔너리에 포함되지 않음.

                if(EventManager.itemNotificationEvent != null)
                    EventManager.itemNotificationEvent(data);

                return true;
            }
            else
            {
                // 인벤토리가 꽉 찼다는 메세지 노출
                return false;
            }
        }

        if (possession.TryGetValue(_itemId, out Collected col))
        {
            if (FindSameItemSlot(_itemId, _value))
            {
                col.value += _value;

                if (EventManager.itemNotificationEvent != null)
                    EventManager.itemNotificationEvent(data);
            }
            else
            {
                // 못찾으면 문제 있음..
                Debug.Log(">>>>> 저 여기 문제 있어요 <<<<<<<");
            }

            return true;
        }
        else
        {
            if (FindEmptySlot(_itemId, _value))
            {
                possession.Add(_itemId, new Collected(_value));

                if (EventManager.itemNotificationEvent != null)
                    EventManager.itemNotificationEvent(data);

                return true;
            }
            else
            {
                // 인벤토리가 꽉 찼을 경우
                // 로그 띄워야 함
                return false;
            }
        }
    }
    public bool FindEmptySlot(int _itemId, int _val)
    {
        for(int i = 0; i < inventorySlot.Length; i++) 
        {
            if (inventorySlot[i].IsEmpty())
            {
                inventorySlot[i].Acquire(GetItemData(_itemId), _val);
                return true;
            }
        }

        return false;
    }
    public bool FindSameItemSlot(int _itemId, int _value)
    {
        for (int i = 0; i < inventorySlot.Length; i++)
        {
            if (inventorySlot[i].IsEmpty()) continue;

            if (inventorySlot[i].Id == _itemId)
            {
                inventorySlot[i].Increase(_value);
                return true;
            }
        }

        return false;
    }
    public bool CheckItemValue(int _itemId, int _value)
    {
        // 아이템이 필요 수량보다 더 존재하는지 확인

        if (possession.TryGetValue(_itemId, out Collected col))
        {
            if (col.value >= _value) return true;
        }

        return false;
    }
    public bool UseItem(int _itemId, int _value)
    {
        // 퀘스트, 기타 등에서 사용처리 해야하는 경우
        if (CheckItemValue(_itemId, _value))
        {
            Debug.Log("CheckItemValue : true");

            List<ItemSlotData> slots = new List<ItemSlotData>();

            int valueCounter = _value;

            for (int i = 0; i < inventorySlot.Length; i++)
            {
                if (inventorySlot[i].IsEmpty()) continue;

                if (inventorySlot[i].Id == _itemId)
                {
                    valueCounter -= inventorySlot[i].Value;

                    slots.Add(inventorySlot[i]);

                    if (valueCounter <= 0) break;
                }
            }

            valueCounter = _value;

            for (int i = 0; i < slots.Count; i++)
            {
                int val = slots[i].Value;

                slots[i].Decrease(Mathf.Min(val, valueCounter));

                valueCounter -= val;

                if(valueCounter <= 0) break;
            }

            if (possession.TryGetValue(_itemId, out Collected col))
            {
                col.value -= _value;

                if (col.value <= 0) possession.Remove(_itemId);
            }

            return true;
        }

        Debug.Log("CheckItemValue : false");


        return false;
    }
    public bool SellItem(int _itemId, int _value)
    {
        // 상점에 판매하는 경우

        if (possession.TryGetValue(_itemId, out Collected col))
        {
            col.value -= _value;

            if (col.value <= 0) possession.Remove(_itemId);

            return true;
        }

        return false;
    }
    public bool UseQuickSlotItem(int _itemId, int _slotId)
    {
        // 퀵슬롯에서의 사용, 매개변수로 slot id 를 받아야 설정한 위치의 아이템을 사용할 수 있다.

        if (possession.TryGetValue(_itemId, out Collected col))
        {
            inventorySlot[_slotId].UseItem(1);

            col.value -= 1;

            if (col.value <= 0) possession.Remove(_itemId);

            return true;
        }

        return false;
    }

    public void ThrowItemAway(Vector3 pos, int itemId, int itemValue)
    {
        EventManager.itemSpawnEvent(pos, itemValue, GetItemObject(itemId));

        SoundManager.sInst.Play("ThrowItemAway");

        if (possession.TryGetValue(itemId, out Collected col))
        {
            col.value -= itemValue;

            if (col.value <= 0) possession.Remove(itemId);
        }
    }
}
