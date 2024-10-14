using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 스토어의 패널에 노출되는 슬롯 UI
/// </summary>
public class StorePanelSlot : ItemSlot
{
    [SerializeField] private ItemData data; // 아이템 정보

    [SerializeField] private Text txt_Price;
    [SerializeField] private Text txt_ItemName;
    [SerializeField] private Image img_item;

    public override void InitializeItemSlot(IItemSlot _eventManager, ItemSlotData _slotData)
    {
        eventManager = _eventManager;

        img_item.sprite = data.Sprite;
        txt_ItemName.text = data.ItemName;
        txt_Price.text = data.GetCurrencyWhenBuy().ToString(); // 구매 시 금액
    }
    
    public override void OnPointerUp(PointerEventData eventData) // UI Event
    {
        eventManager.MerchandiseDoubleClickChecker(this);
    }
    public override void OnPointerEnter(PointerEventData eventData) // UI Event
    {
        eventManager.OnMouseEnter(data, this.transform.position);
    }
    public override void OnPointerExit(PointerEventData eventData) // UI Event
    {
        eventManager.OnMouseExit();
    }
    
    
    public override bool IsSwappable(ItemData compare) => false; // 스토어 슬롯에서는 동작하지 않는다
    public override bool IsEmpty() => false; // 스토어 슬롯에서는 동작하지 않는다
    public override ItemData GetItemData() => data; // 스토어 슬롯에서는 동작하지 않는다
    public override int GetItemValue() => 0; // 스토어 슬롯에서는 동작하지 않는다
    public override ItemSlotData GetSlotData() => null; // 스토어 슬롯에서는 동작하지 않는다
    public override void Swap(ItemData _data, int _value) { } // 스토어 슬롯에서는 동작하지 않는다
    public override void RemoveItem() { } // 스토어 슬롯에서는 동작하지 않는다
    public override void UIUpdate() { } // 스토어 슬롯에서는 동작하지 않는다
    public override void IncreaseValue(int _value) { } // 스토어 슬롯에서는 동작하지 않는다
    public override void DecreaseValue(int _value) { } // 스토어 슬롯에서는 동작하지 않는다
    public override void OnPointerDown(PointerEventData eventData) { } // UI Event // 스토어 슬롯에서는 동작하지 않는다
    public override void OnDrag(PointerEventData eventData) { } // UI Event // 스토어 슬롯에서는 동작하지 않는다

}
