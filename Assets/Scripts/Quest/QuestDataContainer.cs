using System;
using UnityEngine;
using Enums;

[CreateAssetMenu(fileName = "QuestDataContainer", 
    menuName = "ScriptableObject/QuestData/QuestDataContainer", order = 1)]

public class QuestDataContainer : Configurable
{
    [SerializeField] private Quest[] quests;
    public Quest[] GetQuests() => quests;
}

[Serializable]
public class Quest
{
    public NpcType npc;
    public QuestData[] quest;
}
