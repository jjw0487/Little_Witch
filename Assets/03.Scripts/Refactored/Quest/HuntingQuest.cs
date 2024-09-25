using System;
using UnityEngine;

[CreateAssetMenu(fileName = "HuntingQuest", menuName = "ScriptableObject/QuestData/HuntingQuest", order = 1)]
public class HuntingQuest : QuestData
{
    [SerializeField] private MonsterData huntingTarget;
    private Action<int> callback;

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
        if(huntingTarget.Name.Equals(monsterName))
        {
            callback(1);
        }
    }
}


