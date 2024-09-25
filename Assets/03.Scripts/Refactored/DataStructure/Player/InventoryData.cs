using Enums;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Collected
{ // �ߺ� �������� �κ��丮�� ���ҵǾ� ���� ��츦 ����� ������ ���� ����ϱ� ���� 
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
        // ���� ��ȭ
        Gold = PLoad.Load("Gold", 200); // ���� ��ȸ ����� 200��

        // �κ��丮 ����
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
        // ���� ��ųʸ��� ���� ����.
        // �����۳��� ���������� ó������ �ʱ� ���� ( 1���� �� ĭ ���� )
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
        // false�� �����ϴ� ��Ȳ��
        // => �κ��丮�� �� á�� ���.
        // (���� ������ �߰����� �ʰ� �� ���� �� ĭ�� ������.)

        ItemType type = data.Type;

        int _itemId = data.ItemId;

        if (type == Enums.ItemType.Equipment)
        {
            if (FindEmptySlot(_itemId, _value))
            {
                // ���� ��ųʸ��� ���Ե��� ����.

                if(EventManager.itemNotificationEvent != null)
                    EventManager.itemNotificationEvent(data);

                return true;
            }
            else
            {
                // �κ��丮�� �� á�ٴ� �޼��� ����
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
                // ��ã���� ���� ����..
                Debug.Log(">>>>> �� ���� ���� �־�� <<<<<<<");
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
                // �κ��丮�� �� á�� ���
                // �α� ����� ��
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
        // �������� �ʿ� �������� �� �����ϴ��� Ȯ��

        if (possession.TryGetValue(_itemId, out Collected col))
        {
            if (col.value >= _value) return true;
        }

        return false;
    }
    public bool UseItem(int _itemId, int _value)
    {
        // ����Ʈ, ��Ÿ ��� ���ó�� �ؾ��ϴ� ���
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
        // ������ �Ǹ��ϴ� ���

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
        // �����Կ����� ���, �Ű������� slot id �� �޾ƾ� ������ ��ġ�� �������� ����� �� �ִ�.

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
