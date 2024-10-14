using Enums;
using UnityEngine;

/// <summary>
/// �������� ����Ʈ�� ���൵�� �÷��̾��� �ൿ�� ���� ��Ȯ�� ��ϵ� �� �ֵ��� ����Ʈ ���� �� �����Ͽ�
/// ����Ʈ �����͸� �ʿ��� ���� ����
/// </summary>
public class QuestReferenceData
{
    public QuestReferenceData(NpcType _npc, QuestData _data, int _currentValue)
    {
        LoadQuest(_npc, _data, _currentValue);
    }

    private QuestData quest; // ����Ʈ SO ����
    private int currentValue; // goal value �� ���� ����
    private int progress; // ����
    private bool isCleared; // �Ϸ� ����
    private NpcType npc; // Ÿ�� NPC
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

        PSave.Save(quest.PlayerPrefs, currentValue); // �÷��̾������� ����
        Debug.Log($" value  : {currentValue} / {quest.GoalValue}");
    }
}
