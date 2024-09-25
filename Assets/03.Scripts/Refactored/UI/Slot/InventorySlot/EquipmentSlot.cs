using Enums;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : ItemSlot
{
    [SerializeField] private Image img;
    [SerializeField] private EquipmentItemType type; // 현재 이 슬롯에서 허용할 장비 타입

    private ItemSlotData data;

    public override void InitializeItemSlot(IItemSlot _eventManager, ItemSlotData _slotData)
    {
        eventManager = _eventManager;
        data = _slotData;

        UIUpdate();
    }

    public override void UIUpdate()
    {
        if (IsEmpty())
        {
            img.sprite = null;
            img.gameObject.SetActive(false);
        }
        else
        {
            img.sprite = data.sprt;
            img.gameObject.SetActive(true);
        }

    }

    public override void RemoveItem()
    {
        data.Remove();
        img.sprite = null;
        img.gameObject.SetActive(false);
    }

    public override void Swap(ItemData _data, int _value)
    {
        // 아이템 스왑 이벤트
        data.Swap(_data, _value);

        UIUpdate();
    }

    public override bool IsSwappable(ItemData compare)
    {
        // 빈공간이면 true 이지만 상대방에서 호환 안되는 장비일경우 거기서 false를 리턴함
        if (compare == null) return true;
        // 장비가 아니라면 false;
        if (compare.Type != ItemType.Equipment) return false;
        // 장비 중에서 이 슬롯 타입과 맞지 않다면 false;
        if (compare.GetEquipmentType() != type) return false;

        return true;
    }


    public override bool IsEmpty() => data.IsEmpty();
    public override ItemData GetItemData() => data.Data;
    public override ItemSlotData GetSlotData() => data;
    public override int GetItemValue() => data.Value;
    public override void IncreaseValue(int _value) { } // equipment slot 안에서 value의 변화는 없을것이다.
    public override void DecreaseValue(int _value) { } // equipment slot 안에서 value의 변화는 없을것이다.





    public override void OnDrag(PointerEventData eventData)
    {
        if (!IsEmpty()) eventManager.OnDrag();
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!IsEmpty()) eventManager.OnPointerDown(this);
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        if (!IsEmpty()) eventManager.OnPointerUp(eventData);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsEmpty()) eventManager.OnMouseEnter(data.Data, this.transform.position);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (!IsEmpty()) eventManager.OnMouseExit();
    }
}
