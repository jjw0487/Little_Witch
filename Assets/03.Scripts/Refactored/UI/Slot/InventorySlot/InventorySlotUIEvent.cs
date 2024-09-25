using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotUIEvent : IItemSlot
{
    public InventorySlotUIEvent(DragImage _dragImg, InventoryDescriptionPanel _descPanel)
    {
        descPanel = _descPanel;
        dragImg = _dragImg;
    }

    private InventoryDescriptionPanel descPanel;
    private ItemSlot currentSelectedSlot;
    private DragImage dragImg;

    bool isSelected = false;
    bool isDragging = false;

    public void OnMouseEnter(ItemData data, Vector3 pos)
    {
        if (isDragging) return;

        descPanel.On(data.Sprite, data.ItemName, data.OptionDesc, data.Description, pos);
    }

    public void OnMouseExit()
    {
        descPanel.Off();
    }

    public void OnPointerDown(ItemSlot slot)
    {
        isDragging = true;
        descPanel.Off();

        if (slot.GetItemData() != null)
        {
            currentSelectedSlot = slot;
            isSelected = true;

            dragImg.OnPointerDown(currentSelectedSlot.GetItemData().Sprite, Input.mousePosition);
        }
        else
        {
            currentSelectedSlot = null;
            isSelected = false;
        }
    }


    public void OnDrag()
    {
        if (!isSelected) return;

        dragImg.OnDrag(Input.mousePosition);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isSelected) return;

        if (eventData.pointerCurrentRaycast.isValid)
        {

            GameObject target = eventData.pointerCurrentRaycast.gameObject;

            if (target.TryGetComponent(out ItemSlot iSlot))
            {  // ������ ����
                if (iSlot != null)
                {

                    if (SwapItemSlot(currentSelectedSlot, iSlot))
                    {
                        SoundManager.sInst.Play("ButtonClick");
                    }
                    else
                    {
                        Debug.Log("���� ����!");
                    }

                }
            }
            else if (target.TryGetComponent(out QuickItemSlot qSlot))
            { // �� ����
                if (qSlot.IsSwappable(currentSelectedSlot.GetSlotData()))
                {
                    qSlot.Clone(currentSelectedSlot.GetSlotData());
                }
            }

        }
        else // �巡�׵� �������� �˾� �ٱ��� �ξ��� ���
        {
            var obj = UIManager.inst.ShowAndGetPopup("TextLog", false);

            obj.GetComponent<TextLogPopup>().UIUpdate("������ �������� �����ðڽ��ϱ�?", () =>
            {
                Vector3 pos = PlayManager.inst.GetPlayer().Position();

                pos.Set(pos.x, pos.y + 1f, pos.z + 1f);

                DataContainer.sInst.Inventory().ThrowItemAway(pos,
                    currentSelectedSlot.GetItemData().ItemId, currentSelectedSlot.GetItemValue());

                currentSelectedSlot.RemoveItem();
            });
        }

        isDragging = false;
        isSelected = false;
        dragImg.OnPointerUp();
    }


    private bool SwapItemSlot(ItemSlot a, ItemSlot b)
    {
        if (!a.IsSwappable(b.GetItemData())
            || !b.IsSwappable(a.GetItemData())) return false;

        // ���� ����

        ItemData temp = a.GetItemData();
        int tempVal = a.GetItemValue();

        QuickItemSlot a_TempQSlot = a.GetSlotData().connectedQuickSlot;
        QuickItemSlot b_TempQSlot = b.GetSlotData().connectedQuickSlot;

        a.GetSlotData().ChangeQuickSlot(b_TempQSlot);
        b.GetSlotData().ChangeQuickSlot(a_TempQSlot);

        a_TempQSlot?.ChangeSlotData(b.GetSlotData());
        b_TempQSlot?.ChangeSlotData(a.GetSlotData());

        a.Swap(b.GetItemData(), b.GetItemValue());
        b.Swap(temp, tempVal);

        return true;
    }

    public void MerchandiseDoubleClickChecker(ItemSlot slot)
    {
    }
}
