using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// 상점 팝업에서의 슬롯 이벤트 핸들러 ( 드래그, 드롭, 스왑, 포인트 등.. )
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

    private StoreDescriptionPanel descPanel;  // 아이템 설명 패널
    private ItemSlot currentSelectedSlot; // 현재 이벤트 발생한 슬롯
    private DragImage dragImg; // 아이템 드래그시 이미지를 복제하여 노출

    private ItemSlot currentClickedMerchandise; // 더블클릭 체커


    bool isSelected = false;
    bool isDragging = false;

    public void MerchandiseDoubleClickChecker(ItemSlot slot) // 상품에 더블 클릭시 구매 이벤트
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

    private void MerchandiseDoubleClicked(ItemSlot doubleClicked) // 상품에 더블 클릭시 구매 이벤트
    {
        if (store.GetPlayerGold() < doubleClicked.GetItemData().GetCurrencyWhenBuy()) return;

        store.DecisionMaker().ShowStoreDecisionMaker(doubleClicked, 1, true);
    }

    public void OnMouseEnter(ItemData data, Vector3 pos) // 마우스 가져다 대었을 때 아이템 설명
    {
        if (isDragging) return;

        descPanel.On(data, pos);
    }

    public void OnMouseExit() // 마우스가 슬롯 밖으로 나갔을 때 설명 패널 종료
    {
        descPanel.Off();
    }

    public void OnPointerDown(ItemSlot slot) // 마우스가 슬롯 위에서 클릭 되었을 때
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


    public void OnDrag() // 드래그
    {
        if (!isSelected) return;

        dragImg.OnDrag(Input.mousePosition);
    }

    public void OnPointerUp(PointerEventData eventData) // 마우스 떼었을 때
    {
        if (!isSelected) return;

        if (eventData.pointerCurrentRaycast.isValid)
        {
            GameObject target = eventData.pointerCurrentRaycast.gameObject;

            if (target.TryGetComponent(out ItemSlot iSlot))
            {  // 아이템 슬롯
                if (iSlot != null)
                {

                    if (SwapItemSlot(currentSelectedSlot, iSlot))
                    {
                        SoundManager.sInst.Play("ButtonClick");
                    }
                    else
                    {
                        // 상점에서 스왑 실패란 드래그해서 상점으로 옮겼단 말, 즉 판매를 목표로 했으므로
                        // 판매 Textlog를 만들어야 한다.
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

        // 스왑 로직

        ItemData temp = a.GetItemData();
        int tempVal = a.GetItemValue();

        // 퀵슬롯에 등록되어 있는 아이템일 경우 연결된 이벤트를 바꿔주어야 한다.
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
