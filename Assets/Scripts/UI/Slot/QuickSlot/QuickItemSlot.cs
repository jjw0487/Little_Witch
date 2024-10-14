using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuickItemSlot : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private Image img;
    [SerializeField] private Text txt_Value;
    [SerializeField] protected string playerPrefs;

    private QuickItemSlotUIEvent eventManager;
    private ItemSlotData data;
    
    private int slotIdx;
    private bool IsEmpty() => data == null;

    public void InitializeQuickItemSlot(QuickItemSlotUIEvent _eventManager)
    {
        eventManager = _eventManager;

        LoadQuickItemSlot();
    }

    private void LoadQuickItemSlot()
    {
        slotIdx = PLoad.Load(playerPrefs, 100);

        if (slotIdx != 100)
        {
            ItemSlotData data = DataContainer.sInst.Inventory().inventorySlot[slotIdx];

            if (!data.IsEmpty())
            {
                Clone(data);
                
            }
            else
            {
                PSave.Save(playerPrefs, 100);
            }
        }
    }

    public void ChangeSlotData(ItemSlotData _slotData)
    {
        data = _slotData;

        if(data.Value > 0)
        {
            PSave.Save(playerPrefs, data.slotIdx);
        }
        else
        {
            PSave.Save(playerPrefs, 100);
        }
    }

    public void Clone(ItemSlotData _slotData)
    { // 퀵슬롯끼리의 스왑

        data = _slotData;

        if (!data.IsEmpty())
        {
            data.ChangeQuickSlot(this);
            PSave.Save(playerPrefs, data.slotIdx);
        }
        else
        {
            PSave.Save(playerPrefs, 100);
        }
        
        UIUpdate();
    }

    public void UseItem() // listner
    {
        if (IsEmpty()) return;

        if (DataContainer.sInst.Inventory()
            .UseQuickSlotItem(data.Id, data.slotIdx))
        {
            OnValueChanged();
        }
        else
        {
            RemoveData();
        }

    }

    private void OnValueChanged()
    {
        if(data.Value <= 0)
        {
            RemoveData();
            return;
        }

        txt_Value.text = data.Value.ToString();
    }
   
    

    public void RemoveData()
    {
        data = null;

        PSave.Save(playerPrefs, 100);

        UIUpdate();
    }

    public void UIUpdate()
    {
        if(IsEmpty())
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

    public Sprite GetSprite() => data.sprt;
    public ItemSlotData GetSlotData() => data;

    public bool IsSwappable(ItemData _compare)
    {
        if (_compare == null) return true; // 비어있어도 Okay

        return _compare.Type == Enums.ItemType.Consumable;
    }
    public bool IsSwappable(ItemSlotData _slotData)
    {
        if (_slotData == null) return true;

        if (_slotData.IsEmpty()) return true; // 비어있어도 Okay

        return _slotData.Data.Type == Enums.ItemType.Consumable;
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (!IsEmpty()) eventManager.OnDrag();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!IsEmpty()) eventManager.OnPointerDown(eventData, this);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!IsEmpty()) eventManager.OnPointerUp(eventData);
    }

}
