using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 인벤토리 팝업에서의 슬롯 이벤트 핸들러 ( 드래그, 드롭, 스왑, 포인트 등.. )
/// </summary>
public class InventorySlotUIEvent : IItemSlot
{
    public InventorySlotUIEvent(DragImage _dragImg, InventoryDescriptionPanel _descPanel)
    {
        descPanel = _descPanel;
        dragImg = _dragImg;
    }

    private InventoryDescriptionPanel descPanel; // 아이템 설명 패널
    private ItemSlot currentSelectedSlot; // 현재 이벤트 발생한 슬롯
    private DragImage dragImg; // 아이템 드래그시 이미지를 복제하여 노출

    bool isSelected = false;
    bool isDragging = false;

    public void OnMouseEnter(ItemData data, Vector3 pos) // 마우스 가져다 대었을 때 아이템 설명
    {
        if (isDragging) return;

        descPanel.On(data.Sprite, data.ItemName, data.OptionDesc, data.Description, pos);
    }

    public void OnMouseExit() // 마우스가 슬롯 밖으로 나갔을 때 설명 패널 종료
    {
        descPanel.Off();
    }

    public void OnPointerDown(ItemSlot slot) // 마우스가 슬롯 위에서 클릭 되었을 때
    {
        isDragging = true;
        descPanel.Off(); // 아이템 설명 패널 끔

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

            if (target.TryGetComponent(out ItemSlot iSlot)) // 아이템 슬롯에 놓았을 때
            {  
                if (iSlot != null)
                {

                    if (SwapItemSlot(currentSelectedSlot, iSlot))
                    {
                        // 스왑 성공
                        SoundManager.sInst.Play("ButtonClick"); 
                    }

                }
            }
            else if (target.TryGetComponent(out QuickItemSlot qSlot)) // 퀵 슬롯에 놓았을 때
            { 
                if (qSlot.IsSwappable(currentSelectedSlot.GetSlotData()))
                {
                    qSlot.Clone(currentSelectedSlot.GetSlotData());
                }
            }

        }
        else // 드래그된 아이템을 팝업 바깥에 두었을 경우
        {
            // 아이템을 땅에 버릴 때
            var obj = UIManager.inst.ShowAndGetPopup("TextLog", false);

            obj.GetComponent<TextLogPopup>().UIUpdate("정말로 아이템을 버리시겠습니까?", () =>
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

    public void MerchandiseDoubleClickChecker(ItemSlot slot) { } // 상점에서 더블클릭 되었을 때

}
