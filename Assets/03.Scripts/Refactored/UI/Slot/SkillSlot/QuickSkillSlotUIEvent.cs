using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuickSkillSlotUIEvent
{
    public QuickSkillSlotUIEvent(DragImage _dragImg)
    {
        dragImg = _dragImg;
    }

    List<SkillReferenceData> curList = new List<SkillReferenceData>();
    QuickSkillSlot currentSelectedSlot;
    DragImage dragImg;

    bool isSelected = false;

    public void AddSkillList(SkillReferenceData data) => curList.Add(data);
    public void RemoveSkillList(SkillReferenceData data) => curList.Remove(data);
    
    public bool IsSkillExistInList(SkillReferenceData data)
    {
        for(int i = 0; i < curList.Count; i++)
        {
            if (curList[i] == data) return true;
        }

        return false;
    }



    public void OnPointerDown(PointerEventData eventData, QuickSkillSlot slot)
    {

        if (slot.IsCoolTime()) return;

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

            if (target.TryGetComponent(out QuickSkillSlot qSlot))
            { // Äü ½½·Ô

                if (SwapQuickSlot(currentSelectedSlot, qSlot))
                {
                    SoundManager.sInst.Play("ButtonClick");
                }

            }

        }

        isSelected = false;

        dragImg.OnPointerUp();

    }

    private bool SwapQuickSlot(QuickSkillSlot a, QuickSkillSlot b)
    {
        // Äü½½·Ô ³¢¸® ½º¿Ò

        if (!a.IsSwappable(b.GetSlotData())
            || !b.IsSwappable(a.GetSlotData())) return false;


        SkillReferenceData temp = a.GetSlotData();

        a.Clone(b.GetSlotData());
        b.Clone(temp);

        return true;
    }
}
