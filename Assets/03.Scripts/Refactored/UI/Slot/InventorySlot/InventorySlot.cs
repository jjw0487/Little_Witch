using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : ItemSlot
{
    [SerializeField] private Image img;
    [SerializeField] private Text txt_Value;

    private ItemSlotData data;

    public override bool IsEmpty() => data.IsEmpty();
    public override ItemData GetItemData() => data.Data;
    public override int GetItemValue() => data.Value;
    public override ItemSlotData GetSlotData() => data;

    public override void InitializeItemSlot(IItemSlot _eventManager, ItemSlotData _slotData)
    {
        eventManager = _eventManager;

        data = _slotData;

        data.ChangeItemSlot(this);

        UIUpdate();
    }

    public override void Swap(ItemData _data, int _value)
    {
        // ������ ���� �̺�Ʈ
        data.Swap(_data, _value);

        UIUpdate();
    }

    public override void RemoveItem() // <= ������ ������ �̺�Ʈ
    {
        data.Remove();

        UIUpdate();
    }

    public override void UIUpdate()
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

   
    public override bool IsSwappable(ItemData compare)
    {
        // �κ��丮 ������ ��� ������ �����ϴ�.
        // ��� ���԰� Ÿ���� ���� �ʴ� ���� ���ʿ��� �˻��� fales�� ����
        return true;
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
