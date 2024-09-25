using Enums;
using System;
using UnityEngine;

public abstract class QuestData : ScriptableObject
{
    [SerializeField] private string playerPrefs;
    public string PlayerPrefs => playerPrefs;

    [SerializeField] private QuestType type;
    public QuestType Type => type;

    [SerializeField] private int restrictedLevel;
    public int RestrictedLevel => restrictedLevel;

    [SerializeField] private int goalValue;
    public int GoalValue => goalValue;

    // 퀘스트 인벤토리에 안에 퀘스트 탭에서 요구할 변수들
    [SerializeField] private Sprite sprite;
    public Sprite Sprite => sprite;

    [SerializeField] private string questName;
    public string QuestName => questName;

    [SerializeField] private string npc;
    public string Npc => npc;

    [TextArea(3, 10)]
    [SerializeField] private string contents;
    public string Contents => contents;

    [SerializeField] private DialogueData[] dialogue;
    public DialogueData[] Dialogue => dialogue;

    [SerializeField] private Reward[] reward; // => GetReward()

    

    public abstract void AddQuestLister(Action<int> callback);
    public abstract void RemoveQuestListner();

    public void GetReward()
    {
        for(int i = 0; i < reward.Length; i++) 
        {
            RewardType type = reward[i].type;

            if (type == RewardType.Item)
            {
                DataContainer.sInst.Inventory().Acquire(reward[i].item, reward[i].value);
            }
            else if(type == RewardType.Gold)
            {
                DataContainer.sInst.Inventory().Gold = +reward[i].value;
            }
            else if(type == RewardType.Exp)
            {
                DataContainer.sInst.PlayerStatus().EXP = +reward[i].value;
            }
        }
    }
}

[Serializable]
public class Reward
{
    public RewardType type;
    public ItemData item;
    public int value;
}