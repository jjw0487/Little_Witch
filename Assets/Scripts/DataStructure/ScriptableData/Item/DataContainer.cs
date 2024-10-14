using UnityEngine;

/// <summary>
/// 게임이 처음 로드될 때 플레이어 저장 데이터를 클래스화 해서 보관, 필요시 참조하여 사용
/// </summary>
public class DataContainer : Singleton<DataContainer>
{
    [SerializeField] private ItemDataContainer itemDataContainer;
    [SerializeField] private MaxStatDataPerLevel maxDataContainer;
    [SerializeField] private QuestDataContainer questDataContainer;

    private PlayerStatusData playerStat; // 플레이어 스탯 데이터 정보
    private InventoryData inventory; // 플레이어 소지 아이템 정보
    private QuestManager quest; // 퀘스트 진행도
    public InventoryData Inventory() => inventory;
    public PlayerStatusData PlayerStatus() => playerStat;
    public QuestManager Quest() => quest;

    private void OnDisable()
    {
        // 이벤트 등록
        PlayerEvent.additionalStatEvent -= playerStat.ChangeAdditionalStatValue;
        PlayerEvent.levelupCallbackEvent -= quest.LevelUpEvent;
    }


    public void InitializeDataContainer()
    {
        // 게임 실행 시 생성 및 등록
        playerStat = new PlayerStatusData(maxDataContainer);
        PlayerEvent.additionalStatEvent += playerStat.ChangeAdditionalStatValue;
        inventory = new InventoryData(itemDataContainer);
        quest = new QuestManager(questDataContainer, playerStat.Level);
        PlayerEvent.levelupCallbackEvent += quest.LevelUpEvent;
    }
}
