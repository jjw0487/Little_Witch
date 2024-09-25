using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillTreeUIEvent
{
    public SkillTreeUIEvent(DragImage _dragImg, SkillTreeDescriptionPanel _descPanel)
    {
        descPanel = _descPanel;
        dragImg = _dragImg;
    }

    private SkillTreeDescriptionPanel descPanel;
    private SkillPanelSlot currentSelectedSlot;
    private DragImage dragImg;

    private bool isSelected = false;
    bool isDragging = false;

    public void OnMouseEnter(SkillData data, Vector3 pos)
    {
        if (isDragging) return;

        descPanel.On(data, pos);
    }

    public void OnMouseExit()
    {
        descPanel.Off();
    }

    public void OnPointerDown(SkillPanelSlot slot)
    {
        isDragging = true;

        if (slot.GetSkillData() != null)
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

                if (qSlot.IsSwappable(currentSelectedSlot.GetSkillData()))
                {
                    qSlot.Clone(currentSelectedSlot.GetSkillData());
                }
            }

        }

        isSelected = false;
        isDragging = false;
        dragImg.OnPointerUp();

    }
}
