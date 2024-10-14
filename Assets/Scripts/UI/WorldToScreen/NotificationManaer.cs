using UnityEngine;

public class NotificationManaer : MonoBehaviour
{
    [SerializeField] private InteractionNotification interaction;
    [SerializeField] private PoolUnit<ItemAcquiredNotification> item;
    [SerializeField] private PoolUnit<GoldAcquiredNotification> gold;
    [SerializeField] private PoolUnit<QuestAcceptedNotification> questAccepted;
    [SerializeField] private PoolUnit<QuestCompleteNotification> questCompleted;

    private ItemNotificationPooler itemPooler;
    private GoldNotificationPooler goldPooler;
    private QuestNotificationPooler questPooler;

    private void OnDisable()
    {
        EventManager.itemNotificationEvent -= ItemNotification;
        EventManager.goldNotificationEvent -= GoldNotification;
        QuestEvent.questNotificationEvent -= QuestNotificationEvent;
        EventManager.interactionNotificationEvent -= InteractionNotification;
    }

    private void Start()
    {
        InitializeNotificationManager();
    }

    private void InitializeNotificationManager()
    {
        EventManager.interactionNotificationEvent += InteractionNotification;

        itemPooler = new ItemNotificationPooler(item.unit, item.amount, item.parent);

        EventManager.itemNotificationEvent += ItemNotification;

        goldPooler = new GoldNotificationPooler(gold.unit, gold.amount, gold.parent);

        EventManager.goldNotificationEvent += GoldNotification;

        questPooler = new QuestNotificationPooler(questAccepted.unit, questCompleted.unit,
            questAccepted.amount, questAccepted.parent);

        QuestEvent.questNotificationEvent += QuestNotificationEvent;
    }

    private void InteractionNotification(bool active)
    {
        interaction.Spawn(active);
    }
    private void ItemNotification(ItemData item)
    {
        itemPooler.GetObj().Spawn(item);
    }

    private void GoldNotification(int value)
    {
        goldPooler.GetObj().Spawn(value);
    }

    private void QuestNotificationEvent(QuestData quest, bool accepted)
    {
        if (accepted) QuestAcceptedNotification(quest);
        else QuestCompletedNotification(quest);
    }

    private void QuestAcceptedNotification(QuestData quest)
    {
        questPooler.GetAcceptedNotification().Spawn(quest);
    }
    private void QuestCompletedNotification(QuestData quest)
    {
        SoundManager.sInst.Play("QuestCompleted");
        questPooler.GetCompletedNotification().Spawn(quest);
    }

}
