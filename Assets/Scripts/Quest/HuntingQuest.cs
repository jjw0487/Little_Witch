using System;
using UnityEngine;
[CreateAssetMenu(fileName = "HuntingQuest", menuName = "ScriptableObject/QuestData/HuntingQuest", order = 1)]

/// <summary>
/// ��� ����Ʈ ��ũ���ͺ� ������Ʈ
/// </summary>
public class HuntingQuest : QuestData
{
    [SerializeField] private MonsterData huntingTarget; // ��� ���
    private Action<int> callback; // ���൵�� Ȯ���� �Լ� <- QuestReferenceData.cs

    public override void AddQuestLister(Action<int> _callback)
    {
        callback = _callback;

        QuestEvent.huntingQuestEvent += HuntingEvent;
    }

    public override void RemoveQuestListner()
    {
        QuestEvent.huntingQuestEvent -= HuntingEvent;
    }

    public void HuntingEvent(string monsterName)
    {
        if(huntingTarget.Name.Equals(monsterName)) // Ÿ�� ���Ϳ� �̸��� �����Ͽ� ����Ʈ ���൵�� ����
        {
            callback(1);
        }
    }
}


