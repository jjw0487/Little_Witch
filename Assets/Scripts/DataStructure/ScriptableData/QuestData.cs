using Enums;
using System;
using UnityEngine;

/// <summary>
/// 퀘스트 정보 ScriptableObject
/// </summary>
public abstract class QuestData : ScriptableObject
{
    [SerializeField] private string playerPrefs;
    public string PlayerPrefs => playerPrefs;

    [SerializeField] private QuestType type; // 퀘스트 타입
    public QuestType Type => type;

    [SerializeField] private int restrictedLevel; // 퀘스트 제한 레벨
    public int RestrictedLevel => restrictedLevel;

    [SerializeField] private int goalValue; // 성공에 필요한 수치
    public int GoalValue => goalValue;

    // 퀘스트 인벤토리에 안에 퀘스트 탭에서 요구할 변수들
    [SerializeField] private Sprite sprite;
    public Sprite Sprite => sprite;

    [SerializeField] private string questName;
    public string QuestName => questName;

    [SerializeField] private string npc;
    public string Npc => npc;

    [TextArea(3, 10)]
    [SerializeField] private string contents; // 퀘스트 설명
    public string Contents => contents;

    [SerializeField] private DialogueData[] dialogue; // 퀘스트 진행을 위한 대화
    public DialogueData[] Dialogue => dialogue;

    [SerializeField] private Reward[] reward; // 퀘스트 보상 => GetReward()

    

    public abstract void AddQuestLister(Action<int> callback);
    public abstract void RemoveQuestListner();

    public void GetReward() // 퀘스트 성공 후 보상 전달
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