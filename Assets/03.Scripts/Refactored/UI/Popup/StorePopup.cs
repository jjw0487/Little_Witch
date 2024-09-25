using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorePopup : IPopup
{
    [SerializeField] private Button btn_Exit;

    [SerializeField] private Text txt_Gold;

    [SerializeField] private DragImage dragImg;
    [SerializeField] private StoreDescriptionPanel descPanel;

    [SerializeField] private StoreDecisionMaker decisionMaker;

    [SerializeField] private ItemSlot[] inventorySlots;
    [SerializeField] private StorePanelSlot[] storeSlots;

    //
    private InventoryData inventoryData;
    private StoreInventorySlotUIEvent slotEventManager;
    //

    public StoreDecisionMaker DecisionMaker() => decisionMaker;
    public int GetPlayerGold() => inventoryData.Gold;

    private void Start()
    {
        Init();
    }

    public void BuyItem(ItemData data, int value)
    {
        int gold = data.GetCurrencyWhenBuy() * value;

        if (inventoryData.Gold >= gold)
        {
            if (inventoryData.Acquire(data, value))
            {
                inventoryData.Gold = -gold;

                LoadItemSlot();
            }
        }

        
    }

    public void SellItem(ItemSlot slot, int value)
    {
        int gold = slot.GetItemData().GetCurrencyWhenSell() * value;

        if (inventoryData.SellItem(slot.GetItemData().ItemId, value))
        {
            inventoryData.Gold = +gold;
            slot.DecreaseValue(value);
            LoadItemSlot();
        }
    }

    private void Init()
    {
        inventoryData = DataContainer.sInst.Inventory();

        slotEventManager = new StoreInventorySlotUIEvent(dragImg, descPanel, this);

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].InitializeItemSlot(slotEventManager, inventoryData.inventorySlot[i]);
        }

        for (int i = 0; i < storeSlots.Length; i++)
        {
            storeSlots[i].InitializeItemSlot(slotEventManager, null);
        }

        decisionMaker.InitializeStoreDecisionMaker(this);

        txt_Gold.text = inventoryData.Gold.ToString();

        btn_Exit.onClick.AddListener(Exit);

        SlideIn();
    }

    


    private void LoadItemSlot()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].UIUpdate();
        }

        txt_Gold.text = inventoryData.Gold.ToString();
    }

    public override void UIUpdate()
    {
        this.transform.SetAsLastSibling();

        // 게임 플레이중에 습득되거나 소모된 아이템을 다시 로드하여야 한다.
        LoadItemSlot();

        // 재실행
        SlideIn();
    }

    public override void EscapePressed()
    {
        Exit();
    }

    public override void Exit()
    {
        if (isTweening) return;

        SlideOut(() =>
        {
            tr_Body.localPosition = Vector2.zero;
            this.transform.gameObject.SetActive(false);
        });
    }

    
}
