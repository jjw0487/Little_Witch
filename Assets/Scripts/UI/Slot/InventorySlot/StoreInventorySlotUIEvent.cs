using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// ���� �˾������� ���� �̺�Ʈ �ڵ鷯 ( �巡��, ���, ����, ����Ʈ ��.. )
/// </summary>
public class StoreInventorySlotUIEvent : IItemSlot
{
    public StoreInventorySlotUIEvent(DragImage _dragImg, StoreDescriptionPanel _descPanel, StorePopup _store)
    {
        descPanel = _descPanel;
        dragImg = _dragImg;
        store = _store;
    }

    private StorePopup store;

    private StoreDescriptionPanel descPanel;  // ������ ���� �г�
    private ItemSlot currentSelectedSlot; // ���� �̺�Ʈ �߻��� ����
    private DragImage dragImg; // ������ �巡�׽� �̹����� �����Ͽ� ����

    private ItemSlot currentClickedMerchandise; // ����Ŭ�� üĿ


    bool isSelected = false;
    bool isDragging = false;

    public void MerchandiseDoubleClickChecker(ItemSlot slot) // ��ǰ�� ���� Ŭ���� ���� �̺�Ʈ
    {
        if(currentClickedMerchandise == null)
        {
            currentClickedMerchandise = slot;
            return;
        }

        if(slot == currentClickedMerchandise)
        {
            MerchandiseDoubleClicked(slot);
            currentClickedMerchandise = null;
        }
        else currentClickedMerchandise = null;
    }

    private void MerchandiseDoubleClicked(ItemSlot doubleClicked) // ��ǰ�� ���� Ŭ���� ���� �̺�Ʈ
    {
        if (store.GetPlayerGold() < doubleClicked.GetItemData().GetCurrencyWhenBuy()) return;

        store.DecisionMaker().ShowStoreDecisionMaker(doubleClicked, 1, true);
    }

    public void OnMouseEnter(ItemData data, Vector3 pos) // ���콺 ������ ����� �� ������ ����
    {
        if (isDragging) return;

        descPanel.On(data, pos);
    }

    public void OnMouseExit() // ���콺�� ���� ������ ������ �� ���� �г� ����
    {
        descPanel.Off();
    }

    public void OnPointerDown(ItemSlot slot) // ���콺�� ���� ������ Ŭ�� �Ǿ��� ��
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


    public void OnDrag() // �巡��
    {
        if (!isSelected) return;

        dragImg.OnDrag(Input.mousePosition);
    }

    public void OnPointerUp(PointerEventData eventData) // ���콺 ������ ��
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
                        // �������� ���� ���ж� �巡���ؼ� �������� �Ű�� ��, �� �ǸŸ� ��ǥ�� �����Ƿ�
                        // �Ǹ� Textlog�� ������ �Ѵ�.
                        store.DecisionMaker().ShowStoreDecisionMaker(currentSelectedSlot, 
                            currentSelectedSlot.GetItemValue(), false);
                    }
                }
            }

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

        // �����Կ� ��ϵǾ� �ִ� �������� ��� ����� �̺�Ʈ�� �ٲ��־�� �Ѵ�.
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
}
