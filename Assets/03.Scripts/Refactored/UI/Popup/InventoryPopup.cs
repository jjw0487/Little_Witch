using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPopup : IPopup
{
    [SerializeField] private Button btn_Exit;

    [SerializeField] private Text txt_Gold;

    [SerializeField] private DragImage dragImg;
    [SerializeField] private InventoryDescriptionPanel descPanel;
    [SerializeField] private InventoryPlayerStatusPanel statPanel;

    [SerializeField] private ItemSlot[] inventorySlots;
    [SerializeField] private ItemSlot[] equipmentSlots;

    //
    private InventoryData inventoryData;
    private InventorySlotUIEvent slotEventManager;
    //

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        inventoryData = DataContainer.sInst.Inventory();

        slotEventManager = new InventorySlotUIEvent(dragImg, descPanel);

        for(int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].InitializeItemSlot(slotEventManager, inventoryData.inventorySlot[i]);
        }

        for(int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].InitializeItemSlot(slotEventManager, inventoryData.equipmentSlot[i]);
        }

        txt_Gold.text = inventoryData.Gold.ToString();

        statPanel.Initialize(DataContainer.sInst.PlayerStatus());

        btn_Exit.onClick.AddListener(Exit);

        SlideIn();
    }

    private void LoadItemSlot()
    {

        for(int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].UIUpdate();
        }

        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].UIUpdate();
        }

        txt_Gold.text = inventoryData.Gold.ToString();
    }

    public override void UIUpdate()
    {
        this.transform.SetAsLastSibling();

        // 게임 플레이중에 습득되거나 소모된 아이템을 다시 로드하여야 한다.
        LoadItemSlot();

        // 스텟 내용 업데이트
        statPanel.UIUpdate();

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
