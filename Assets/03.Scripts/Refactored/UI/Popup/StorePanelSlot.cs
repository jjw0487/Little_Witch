using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StorePanelSlot : ItemSlot
{
    [SerializeField] private ItemData data;

    [SerializeField] private Text txt_Price;
    [SerializeField] private Text txt_ItemName;
    [SerializeField] private Image img_item;

    public override void InitializeItemSlot(IItemSlot _eventManager, ItemSlotData _slotData)
    {
        eventManager = _eventManager;

        img_item.sprite = data.Sprite;
        txt_ItemName.text = data.ItemName;
        txt_Price.text = data.GetCurrencyWhenBuy().ToString();

        UIUpdate();
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        //eventManager.OnPointerDown(this);
    }
    public override void OnDrag(PointerEventData eventData)
    {
        //eventManager.OnDrag();
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        //eventManager.OnPointerUp(eventData);
        eventManager.MerchandiseDoubleClickChecker(this);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        eventManager.OnMouseEnter(data, this.transform.position);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        eventManager.OnMouseExit();
    }

    public override void RemoveItem() { }
    public override void UIUpdate() { }
    public override void Swap(ItemData _data, int _value) { }
    public override bool IsSwappable(ItemData compare) => false;
    public override void IncreaseValue(int _value) { }
    public override void DecreaseValue(int _value) { }
    public override bool IsEmpty() => false;
    public override ItemData GetItemData() => data;
    public override int GetItemValue() => 0;
    public override ItemSlotData GetSlotData() => null;

}
