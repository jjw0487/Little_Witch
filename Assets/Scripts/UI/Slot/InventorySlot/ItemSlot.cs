using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ItemSlot : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    protected IItemSlot eventManager;
    public abstract void InitializeItemSlot(IItemSlot _eventManager, ItemSlotData _slotData);
    public abstract void UIUpdate();
    public abstract void Swap(ItemData _data, int _value);
    public abstract ItemData GetItemData();
    public abstract ItemSlotData GetSlotData();
    public abstract int GetItemValue();
    public abstract void RemoveItem();

    public abstract void IncreaseValue(int _value);
    public abstract void DecreaseValue(int _value);

    public abstract bool IsSwappable(ItemData _compare);
    public abstract bool IsEmpty();

    public abstract void OnPointerDown(PointerEventData eventData);
    public abstract void OnDrag(PointerEventData eventData);
    public abstract void OnPointerUp(PointerEventData eventData);
    public abstract void OnPointerEnter(PointerEventData eventData);
    public abstract void OnPointerExit(PointerEventData eventData);

}
