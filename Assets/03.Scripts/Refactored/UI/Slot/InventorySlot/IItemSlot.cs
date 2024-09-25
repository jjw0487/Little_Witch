using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IItemSlot
{
    public void OnMouseEnter(ItemData data, Vector3 pos);
    public void OnMouseExit();
    public void OnPointerDown(ItemSlot slot);
    public void OnDrag();
    public void OnPointerUp(PointerEventData eventData);
    public void MerchandiseDoubleClickChecker(ItemSlot slot);
}
