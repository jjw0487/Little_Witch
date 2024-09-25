using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataContainer : Singleton<DataContainer>
{
    [SerializeField] private ItemDataContainer itemDataContainer;
    [SerializeField] private MaxStatDataPerLevel maxDataContainer;
    [SerializeField] private QuestDataContainer questDataContainer;

    private PlayerStatusData playerStat;
    private InventoryData inventory;
    private QuestManager quest;
    public InventoryData Inventory() => inventory;
    public PlayerStatusData PlayerStatus() => playerStat;
    public QuestManager Quest() => quest;

    private void OnDisable()
    {
        PlayerEvent.additionalStatEvent -= playerStat.ChangeAdditionalStatValue;
        PlayerEvent.levelupCallbackEvent -= quest.LevelUpEvent;
    }


    public void InitializeDataContainer()
    {
        // order 1 => 플레이어 스텟과 ChangeAdditionalStatValue() 이벤트를 생성하고
        playerStat = new PlayerStatusData(maxDataContainer);
        PlayerEvent.additionalStatEvent += playerStat.ChangeAdditionalStatValue;

        // order 2 => 인벤토리 데이터를 생성하면서 ChangeAdditionalStatValue() 에 정보를 입력
        inventory = new InventoryData(itemDataContainer);
        
        quest = new QuestManager(questDataContainer, playerStat.Level);
        PlayerEvent.levelupCallbackEvent += quest.LevelUpEvent;
    }
}
