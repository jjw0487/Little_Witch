using System;
using UnityEngine;
[CreateAssetMenu(fileName = "HuntingQuest", menuName = "ScriptableObject/QuestData/HuntingQuest", order = 1)]

/// <summary>
/// 사냥 퀘스트 스크립터블 오브젝트
/// </summary>
public class HuntingQuest : QuestData
{
    [SerializeField] private MonsterData huntingTarget; // 사냥 대상
    private Action<int> callback; // 진행도를 확인할 함수 <- QuestReferenceData.cs

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
        if(huntingTarget.Name.Equals(monsterName)) // 타겟 몬스터와 이름을 대조하여 퀘스트 진행도를 변경
        {
            callback(1);
        }
    }
}


