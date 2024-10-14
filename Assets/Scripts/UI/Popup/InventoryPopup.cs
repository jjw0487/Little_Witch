using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 인벤토리 팝업 내에서 발생하는 이벤트
/// </summary>
public class InventoryPopup : IPopup
{
    [SerializeField] private Button btn_Exit; // X 버튼

    [SerializeField] private Text txt_Gold; // 골드 표시

    [SerializeField] private InventoryDescriptionPanel descPanel; // 아이템 설명 패널
    [SerializeField] private InventoryPlayerStatusPanel statPanel; // 플레이어 스텟 표시
    [SerializeField] private DragImage dragImg; // 드래그 시 타겟 이미지를 반투명으로 복사하여 표현

    [SerializeField] private ItemSlot[] inventorySlots; // 아이템 슬롯
    [SerializeField] private ItemSlot[] equipmentSlots; // 장비 아이템 슬롯

    //
    private InventoryData inventoryData; // 습득, 재실행, 씬 전환 등으로 변경된 아이템의 수량, 위치 정보

    private InventorySlotUIEvent slotEventManager; // 인벤토리 팝업 내에 슬롯 이벤트를 총괄
    //

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        inventoryData = DataContainer.sInst.Inventory(); // 저장된 인벤토리 데이터를 받음

        slotEventManager = new InventorySlotUIEvent(dragImg, descPanel); // Slot의 UI Event Manager

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

    private void LoadItemSlot() // 아이템의 수량이나 위치의 변화가 있었다면 업데이트
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

    public override void EscapePressed() // Escape Key 이벤트로 팝업이 꺼졌을 때
    {
        Exit();
    }

    public override void Exit() // 팝업 종료
    {
        if (isTweening) return;

        SlideOut(() => 
        {
            tr_Body.localPosition = Vector2.zero;
            this.transform.gameObject.SetActive(false);
        });
    }
   
}
