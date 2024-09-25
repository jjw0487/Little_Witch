using UnityEngine;
using Enums;
using System;
using UnityEngine.Analytics;

/// <summary>
/// �κ��丮 ���԰� �����Կ� ���� ������ ����, �����Ѵ�.
/// </summary>

public class ItemSlotData
{
    public ItemSlotData(SlotType _type, int _slotIdx)
    {
        slotIdx = _slotIdx;
        slotType = _type;
    }
    public ItemSlotData(SlotType _type, int _slotIdx, ItemData _data, int _value)
    {
        this.slotIdx = _slotIdx;
        slotType = _type;

        if (_value <= 0) Remove();
        else Acquire(_data, _value);
    }
    private ItemSlot connectedItemSlot; // �κ��丮���� ��ü�Ǵ� ������
    public QuickItemSlot connectedQuickSlot; // GUI ������ ������

    public int slotIdx;
    private SlotType slotType;
    private ItemData data;
    private int value;

    public int Id => data.ItemId;
    public Sprite sprt => data.Sprite;
    public ItemData Data => data;
    public string Description => data.Description;
    public int Value => value;
    public bool IsEmpty() => value <= 0;
    public void ChangeItemSlot(ItemSlot _slot) => connectedItemSlot = _slot;
    public void ChangeQuickSlot(QuickItemSlot _quickSlot) => connectedQuickSlot = _quickSlot;

    public void Swap(ItemData _data, int _value)
    { // ������ ��ġ ��ü
        if (_value <= 0) Remove();
        else Acquire(_data, _value);
    }

    public void Acquire(ItemData _data, int _value)
    { // ���ο� ������ ȹ��
        data = _data;
        value = _value;

        if (slotType == SlotType.Equipment)
        {
            // (+)data.GetOptionValue()
            PlayerEvent.additionalStatEvent(data.GetAdditionalStatType(), data.GetOptionValue());

            if (EventManager.uiUpdateEvent != null) EventManager.uiUpdateEvent();
        }

        PSave.Save($"Slot_{slotIdx}", $"{Id}_{Value}");
    }

    public void Remove()
    {
        if(value > 0)
        { 
            if(slotType == SlotType.Equipment)
            {
                // (-)data.GetOptionValue()
                PlayerEvent.additionalStatEvent(data.GetAdditionalStatType(), -data.GetOptionValue());

                if (EventManager.uiUpdateEvent != null) EventManager.uiUpdateEvent();
            }
        }

        data = null;
        value = 0;

        connectedItemSlot = null;
        connectedQuickSlot = null;

        PSave.Save($"Slot_{slotIdx}", "");
    }

    public void Increase(int _value)
    { // ������ ���� ����
        value += _value;

        OnUIValueChanged();

        PSave.Save($"Slot_{slotIdx}", $"{Id}_{Value}");
    }

    public void Decrease(int _value)
    { // ������ ���� ����
        value -= _value;

        OnUIValueChanged();

        if (!IsEmpty()) PSave.Save($"Slot_{slotIdx}", $"{Id}_{Value}");
        else Remove();
    }

    public void UseItem(int _value)
    { // ������ ���� ����
        if (data.Type == ItemType.Consumable)
        {
            SoundManager.sInst.Play("UsePotionItem");

            DataContainer.sInst.PlayerStatus().RecoveryStatValue
                (data.GetConsumableType(), data.GetOptionValue());
        }

        Decrease(_value); 
    }

    private void OnUIValueChanged()
    {
        if (connectedItemSlot != null) connectedItemSlot.UIUpdate();
        if (connectedQuickSlot != null) connectedQuickSlot.UIUpdate();
    }

    

}
