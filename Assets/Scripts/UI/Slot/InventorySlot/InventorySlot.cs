using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 일반 아이템 슬롯
/// </summary>
public class InventorySlot : ItemSlot
{
    [SerializeField] private Image img;
    [SerializeField] private Text txt_Value; // 수량

    private ItemSlotData data; // 저장된 슬롯 데이터

    public override bool IsEmpty() => data.IsEmpty();
    public override ItemData GetItemData() => data.Data;
    public override int GetItemValue() => data.Value;
    public override ItemSlotData GetSlotData() => data;

    public override bool IsSwappable(ItemData compare)
    {
        // 인벤토리 슬롯은 모든 스왑이 가능하다.
        // 장비 슬롯과 타입이 맞지 않는 경우는 그쪽에서 검사후 fales를 리턴
        return true;
    }

    public override void InitializeItemSlot(IItemSlot _eventManager, ItemSlotData _slotData)
    {
        eventManager = _eventManager;

        data = _slotData;

        data.ChangeItemSlot(this);

        UIUpdate();
    }

    public override void Swap(ItemData _data, int _value) // 아이템 스왑 이벤트
    {
        data.Swap(_data, _value);

        UIUpdate();
    }

    public override void RemoveItem() // <= 아이템 버리는 이벤트
    {
        data.Remove();

        UIUpdate();
    }

    public override void UIUpdate() // 인벤토리에 표시되는 UI 업데이트
    {
        if (IsEmpty())
        {
            img.sprite = null;
            img.gameObject.SetActive(false);
            txt_Value.text = "";
        }
        else
        {
            img.sprite = data.sprt;
            img.gameObject.SetActive(true);
            txt_Value.text = data.Value.ToString();
        }
    }

    public override void IncreaseValue(int _value)
    {
        data.Increase(_value);

        UIUpdate();
    }

    public override void DecreaseValue(int _value)
    {
        data.Decrease(_value);

        UIUpdate();
    }

    public override void OnDrag(PointerEventData eventData) // UI Event
    {
        if (!IsEmpty()) eventManager.OnDrag();
    }
    public override void OnPointerDown(PointerEventData eventData) // UI Event
    {
        if (!IsEmpty()) eventManager.OnPointerDown(this);
    }
    public override void OnPointerUp(PointerEventData eventData) // UI Event
    {
        if (!IsEmpty()) eventManager.OnPointerUp(eventData);
    }
    public override void OnPointerEnter(PointerEventData eventData) // UI Event
    {
        if (!IsEmpty()) eventManager.OnMouseEnter(data.Data, this.transform.position);
    }
    public override void OnPointerExit(PointerEventData eventData) // UI Event
    {
        if (!IsEmpty()) eventManager.OnMouseExit();
    }
}
