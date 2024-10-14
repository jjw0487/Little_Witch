using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// ���� �˾�
/// </summary>
public class StorePopup : IPopup
{
    [SerializeField] private Button btn_Exit;

    [SerializeField] private Text txt_Gold; // ��� �ؽ�Ʈ

    [SerializeField] private DragImage dragImg; // �巡�� ��� �̹���

    [SerializeField] private StoreDescriptionPanel descPanel; // ���� �г�

    [SerializeField] private StoreDecisionMaker decisionMaker; // ������ �߻��ϴ� �̺�Ʈ UI ǥ�� ( ������ ����, �Ǹ� )

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

    public void BuyItem(ItemData data, int value) // ������ ���� �̺�Ʈ
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

    public void SellItem(ItemSlot slot, int value) // ������ �Ǹ� �̺�Ʈ
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

        // ���� �÷����߿� ����ǰų� �Ҹ�� �������� �ٽ� �ε��Ͽ��� �Ѵ�.
        LoadItemSlot();

        // �����
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
