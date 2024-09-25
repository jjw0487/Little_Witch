using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuickItemSlotUIEvent
{
    public QuickItemSlotUIEvent(DragImage _dragImg)
    {
        dragImg = _dragImg;
    }

    QuickItemSlot currentSelectedSlot;
    DragImage dragImg;

    bool isSelected = false;

    public void OnPointerDown(PointerEventData eventData, QuickItemSlot slot)
    {

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            slot.RemoveData();
            return;
        }

        if (slot.GetSlotData() != null)
        {
            currentSelectedSlot = slot;
            isSelected = true;

            dragImg.OnPointerDown(currentSelectedSlot.GetSprite(), Input.mousePosition);
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

            if (target.TryGetComponent(out QuickItemSlot qSlot))
            { // Äü ½½·Ô

                if (SwapQuickSlot(currentSelectedSlot, qSlot))
                {
                    Debug.Log("Äü½½·Ô ½º¿Ò ¼º°ø !");
                }

            }

        }

        isSelected = false;

        dragImg.OnPointerUp();

    }

    private bool SwapQuickSlot(QuickItemSlot a, QuickItemSlot b)
    {
        // Äü½½·Ô ³¢¸® ½º¿Ò

        if (!a.IsSwappable(b.GetSlotData())
            || !b.IsSwappable(a.GetSlotData())) return false;

        
        ItemSlotData temp = a.GetSlotData();

        a.Clone(b.GetSlotData());
        b.Clone(temp);

        return true;
    }
}
