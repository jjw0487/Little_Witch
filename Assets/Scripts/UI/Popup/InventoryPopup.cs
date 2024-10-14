using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �κ��丮 �˾� ������ �߻��ϴ� �̺�Ʈ
/// </summary>
public class InventoryPopup : IPopup
{
    [SerializeField] private Button btn_Exit; // X ��ư

    [SerializeField] private Text txt_Gold; // ��� ǥ��

    [SerializeField] private InventoryDescriptionPanel descPanel; // ������ ���� �г�
    [SerializeField] private InventoryPlayerStatusPanel statPanel; // �÷��̾� ���� ǥ��
    [SerializeField] private DragImage dragImg; // �巡�� �� Ÿ�� �̹����� ���������� �����Ͽ� ǥ��

    [SerializeField] private ItemSlot[] inventorySlots; // ������ ����
    [SerializeField] private ItemSlot[] equipmentSlots; // ��� ������ ����

    //
    private InventoryData inventoryData; // ����, �����, �� ��ȯ ������ ����� �������� ����, ��ġ ����

    private InventorySlotUIEvent slotEventManager; // �κ��丮 �˾� ���� ���� �̺�Ʈ�� �Ѱ�
    //

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        inventoryData = DataContainer.sInst.Inventory(); // ����� �κ��丮 �����͸� ����

        slotEventManager = new InventorySlotUIEvent(dragImg, descPanel); // Slot�� UI Event Manager

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

    private void LoadItemSlot() // �������� �����̳� ��ġ�� ��ȭ�� �־��ٸ� ������Ʈ
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

        // ���� �÷����߿� ����ǰų� �Ҹ�� �������� �ٽ� �ε��Ͽ��� �Ѵ�.
        LoadItemSlot();

        // ���� ���� ������Ʈ
        statPanel.UIUpdate();

        // �����
        SlideIn();
    }

    public override void EscapePressed() // Escape Key �̺�Ʈ�� �˾��� ������ ��
    {
        Exit();
    }

    public override void Exit() // �˾� ����
    {
        if (isTweening) return;

        SlideOut(() => 
        {
            tr_Body.localPosition = Vector2.zero;
            this.transform.gameObject.SetActive(false);
        });
    }
   
}
