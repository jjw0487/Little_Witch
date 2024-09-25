using Enums;
using UnityEngine;
public class QuestReferenceData
{
    public QuestReferenceData(NpcType _npc, QuestData _data, int _currentValue)
    {
        LoadQuest(_npc, _data, _currentValue);
    }

    private QuestData quest;
    private int currentValue; // goal value 와 비교할 수량
    private int progress;
    private bool isCleared;
    private NpcType npc;
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
            { // 획득 하였다가 버리거나 취소한 경우
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

        PSave.Save(quest.PlayerPrefs, currentValue);
        Debug.Log($" value  : {currentValue} / {quest.GoalValue}");
    }
}
