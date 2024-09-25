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
        // order 1 => �÷��̾� ���ݰ� ChangeAdditionalStatValue() �̺�Ʈ�� �����ϰ�
        playerStat = new PlayerStatusData(maxDataContainer);
        PlayerEvent.additionalStatEvent += playerStat.ChangeAdditionalStatValue;

        // order 2 => �κ��丮 �����͸� �����ϸ鼭 ChangeAdditionalStatValue() �� ������ �Է�
        inventory = new InventoryData(itemDataContainer);
        
        quest = new QuestManager(questDataContainer, playerStat.Level);
        PlayerEvent.levelupCallbackEvent += quest.LevelUpEvent;
    }
}
