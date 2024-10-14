using UnityEngine;

/// <summary>
/// ������ ó�� �ε�� �� �÷��̾� ���� �����͸� Ŭ����ȭ �ؼ� ����, �ʿ�� �����Ͽ� ���
/// </summary>
public class DataContainer : Singleton<DataContainer>
{
    [SerializeField] private ItemDataContainer itemDataContainer;
    [SerializeField] private MaxStatDataPerLevel maxDataContainer;
    [SerializeField] private QuestDataContainer questDataContainer;

    private PlayerStatusData playerStat; // �÷��̾� ���� ������ ����
    private InventoryData inventory; // �÷��̾� ���� ������ ����
    private QuestManager quest; // ����Ʈ ���൵
    public InventoryData Inventory() => inventory;
    public PlayerStatusData PlayerStatus() => playerStat;
    public QuestManager Quest() => quest;

    private void OnDisable()
    {
        // �̺�Ʈ ���
        PlayerEvent.additionalStatEvent -= playerStat.ChangeAdditionalStatValue;
        PlayerEvent.levelupCallbackEvent -= quest.LevelUpEvent;
    }


    public void InitializeDataContainer()
    {
        // ���� ���� �� ���� �� ���
        playerStat = new PlayerStatusData(maxDataContainer);
        PlayerEvent.additionalStatEvent += playerStat.ChangeAdditionalStatValue;
        inventory = new InventoryData(itemDataContainer);
        quest = new QuestManager(questDataContainer, playerStat.Level);
        PlayerEvent.levelupCallbackEvent += quest.LevelUpEvent;
    }
}
