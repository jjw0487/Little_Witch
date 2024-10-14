using Enums;
using UnityEngine;

/// <summary>
/// 진행중인 퀘스트의 진행도가 플레이어의 행동에 따라 정확히 기록될 수 있도록 퀘스트 진행 시 생성하여
/// 퀘스트 데이터를 필요한 곳에 전달
/// </summary>
public class QuestReferenceData
{
    public QuestReferenceData(NpcType _npc, QuestData _data, int _currentValue)
    {
        LoadQuest(_npc, _data, _currentValue);
    }

    private QuestData quest; // 퀘스트 SO 정보
    private int currentValue; // goal value 와 비교할 수량
    private int progress; // 진행
    private bool isCleared; // 완료 여부
    private NpcType npc; // 타겟 NPC
    public DialogueData GetCurrentDialogue() => quest.Dialogue[progress];
    public int GetCurrentValue() => currentValue;
    public bool IsCleared() => isCleared;
    public QuestData GetData() => quest;
    private void LoadQuest(NpcType _npc,QuestData _data, int _currentValue)
    {
        npc = _npc;
        quest = _data;
        currentValue = _currentValue;
        progress = 1;
        isCleared = false;
        quest.AddQuestLister(OnQuestValueChanged);
        PSave.Save(quest.PlayerPrefs, currentValue);
    }
    public void QuestCompleted()
    {
        PSave.Save(quest.PlayerPrefs, 200);
        quest.RemoveQuestListner();
        quest.GetReward();
    }
    public void OnQuestValueChanged(int _value)
    {
        currentValue += _value;

        if(currentValue < quest.GoalValue)
        {
            if(isCleared)
            {
                progress--;
                isCleared = false;

                DataContainer.sInst.Quest().QuestNotification(npc, false);
            }
        }

        if(currentValue >= quest.GoalValue)
        {
            if(!isCleared)
            {
                progress++;
                isCleared = true;
                QuestEvent.questNotificationEvent(quest, false);

                DataContainer.sInst.Quest().QuestNotification(npc, true);
            }
        }

        PSave.Save(quest.PlayerPrefs, currentValue); // 플레이어프랩스 저장
        Debug.Log($" value  : {currentValue} / {quest.GoalValue}");
    }
}
