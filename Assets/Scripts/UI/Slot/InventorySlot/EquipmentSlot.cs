using Enums;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// ��� ������ ����
/// </summary>
public class EquipmentSlot : ItemSlot
{
    [SerializeField] private Image img;
    [SerializeField] private EquipmentItemType type; // ���� �� ���Կ��� ����� ��� Ÿ��

    private ItemSlotData data; // ����� ���� ������

    public override bool IsEmpty() => data.IsEmpty();
    public override ItemData GetItemData() => data.Data;
    public override ItemSlotData GetSlotData() => data;
    public override int GetItemValue() => data.Value;
    public override void IncreaseValue(int _value) { } // equipment slot �ȿ��� value�� ��ȭ ����
    public override void DecreaseValue(int _value) { } // equipment slot �ȿ��� value�� ��ȭ ����

    public override bool IsSwappable(ItemData compare)
    {
        // ������̸� true ������ ���濡�� ȣȯ �ȵǴ� ����ϰ�� �ű⼭ false�� ������
        if (compare == null) return true;
        // ��� �ƴ϶�� false;
        if (compare.Type != ItemType.Equipment) return false;
        // ��� �߿��� �� ���� Ÿ�԰� ���� �ʴٸ� false;
        if (compare.GetEquipmentType() != type) return false;

        return true;
    }

    public override void InitializeItemSlot(IItemSlot _eventManager, ItemSlotData _slotData)
    {
        eventManager = _eventManager;
        data = _slotData;

        UIUpdate();
    }

    public override void UIUpdate() // �κ��丮�� ǥ�õǴ� UI ������Ʈ
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
        // ������ ���� �̺�Ʈ
        data.Swap(_data, _value);

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
