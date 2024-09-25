using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShortcutSlotUIEvent
{
    public ShortcutSlotUIEvent(DragText _dragText)
    {
        dragText = _dragText;
    }

    private ShortcutSlot currentSelectedSlot;
    private DragText dragText;

    bool isSelected = false;

    private bool SwapItemSlot(ShortcutSlot a, ShortcutSlot b)
    {
        // shortcut은 항상 true를 리턴하긴 하는데...
        if (!a.IsSwappable()
            || !b.IsSwappable()) return false;

        // 스왑 로직
        KeyEvent temp = a.GetData();
        a.Swap(b.GetData());
        b.Swap(temp);

        return true;
    }
    public void OnPointerDown(ShortcutSlot slot)
    {
        if (!slot.IsEmpty())
        {
            currentSelectedSlot = slot;
            isSelected = true;

            dragText.OnPointerDown(currentSelectedSlot.GetData().GetEventName(), Input.mousePosition);
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

        dragText.OnDrag(Input.mousePosition);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isSelected) return;

        if (eventData.pointerCurrentRaycast.isValid)
        {
            GameObject target = eventData.pointerCurrentRaycast.gameObject;

            if (target.TryGetComponent(out ShortcutSlot sSlot))
            { 
                if (sSlot != null)
                {
                    if (SwapItemSlot(currentSelectedSlot, sSlot))
                    {
                        SoundManager.sInst.Play("ButtonClick");
                    }
                }
            }
        }

        isSelected = false;
        dragText.OnPointerUp();
    }


    
}
