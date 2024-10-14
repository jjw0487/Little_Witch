using Enums;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DeliveryQuest", menuName = "ScriptableObject/QuestData/DeliveryQuest", order = 1)]

/// <summary>
/// 배달 퀘스트 스크립터블 오브젝트
/// </summary>
public class DeliveryQuest : QuestData
{
    [SerializeField] private ItemData targetItem; // 배달 타겟 아이템
    [SerializeField] private int targetItemValue; // 수량
    [SerializeField] private NpcType deliveryTarget; // 배달 대상

    [SerializeField] private DialogueData addresseeDialogue; // 배달 대상 수령 후 Dialogue

    private Action<int> callback; // 진행도를 확인할 함수 <- QuestReferenceData.cs

    public override void AddQuestLister(Action<int> _callback)
    {
        callback = _callback;
        QuestEvent.deliveryQuestEvent += DeliveryEvent;
    }

    public override void RemoveQuestListner()
    {
        QuestEvent.deliveryQuestEvent -= DeliveryEvent;
    }

    public bool DeliveryEvent(NpcType _deliveryTarget, out DialogueData dialogue)
    {
        if (deliveryTarget == _deliveryTarget)
        {
            if(DataContainer.sInst.Inventory().UseItem(targetItem.ItemId, targetItemValue))
            {
                dialogue = addresseeDialogue;
                callback(1);
                return true;
            }
        }
        dialogue = null;
        return false;
    }
}
