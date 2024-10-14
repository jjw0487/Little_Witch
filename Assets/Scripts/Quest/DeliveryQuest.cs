using Enums;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DeliveryQuest", menuName = "ScriptableObject/QuestData/DeliveryQuest", order = 1)]

/// <summary>
/// ��� ����Ʈ ��ũ���ͺ� ������Ʈ
/// </summary>
public class DeliveryQuest : QuestData
{
    [SerializeField] private ItemData targetItem; // ��� Ÿ�� ������
    [SerializeField] private int targetItemValue; // ����
    [SerializeField] private NpcType deliveryTarget; // ��� ���

    [SerializeField] private DialogueData addresseeDialogue; // ��� ��� ���� �� Dialogue

    private Action<int> callback; // ���൵�� Ȯ���� �Լ� <- QuestReferenceData.cs

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
