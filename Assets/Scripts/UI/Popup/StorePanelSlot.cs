using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// ������� �гο� ����Ǵ� ���� UI
/// </summary>
public class StorePanelSlot : ItemSlot
{
    [SerializeField] private ItemData data; // ������ ����

    [SerializeField] private Text txt_Price;
    [SerializeField] private Text txt_ItemName;
    [SerializeField] private Image img_item;

    public override void InitializeItemSlot(IItemSlot _eventManager, ItemSlotData _slotData)
    {
        eventManager = _eventManager;

        img_item.sprite = data.Sprite;
        txt_ItemName.text = data.ItemName;
        txt_Price.text = data.GetCurrencyWhenBuy().ToString(); // ���� �� �ݾ�
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
    
    
    public override bool IsSwappable(ItemData compare) => false; // ����� ���Կ����� �������� �ʴ´�
    public override bool IsEmpty() => false; // ����� ���Կ����� �������� �ʴ´�
    public override ItemData GetItemData() => data; // ����� ���Կ����� �������� �ʴ´�
    public override int GetItemValue() => 0; // ����� ���Կ����� �������� �ʴ´�
    public override ItemSlotData GetSlotData() => null; // ����� ���Կ����� �������� �ʴ´�
    public override void Swap(ItemData _data, int _value) { } // ����� ���Կ����� �������� �ʴ´�
    public override void RemoveItem() { } // ����� ���Կ����� �������� �ʴ´�
    public override void UIUpdate() { } // ����� ���Կ����� �������� �ʴ´�
    public override void IncreaseValue(int _value) { } // ����� ���Կ����� �������� �ʴ´�
    public override void DecreaseValue(int _value) { } // ����� ���Կ����� �������� �ʴ´�
    public override void OnPointerDown(PointerEventData eventData) { } // UI Event // ����� ���Կ����� �������� �ʴ´�
    public override void OnDrag(PointerEventData eventData) { } // UI Event // ����� ���Կ����� �������� �ʴ´�

}
