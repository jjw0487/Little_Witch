using Enums;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class QuestManager
{
    public QuestManager(QuestDataContainer _quests, int _playerLevel)
    {
        quest = _quests.GetQuests();

        playerLevel = _playerLevel;

        InitializeQuestManager();
    }

    // 진행 중인 퀘스트
    private Dictionary<NpcType, QuestReferenceData> ongoingQuest 
        = new Dictionary<NpcType, QuestReferenceData>();

    // 클리어 되지 않은 퀘스트
    private Dictionary<NpcType, List<QuestData>> notCompleted 
        = new Dictionary<NpcType, List<QuestData>>();

    // 퀘스트 알림을 넘겨줄 Npc 저장
    private Dictionary<NpcType, INpc> npcListner 
        = new Dictionary<NpcType, INpc>();

    private Quest[] quest;

    private int playerLevel;

    public Dictionary<NpcType, QuestReferenceData> GetOngoingQuest() => ongoingQuest;

    private void InitializeQuestManager()
    {
        for(int i = 0; i < quest.Length; i++)
        {
            NpcType key = quest[i].npc;

            for (int n = 0; n < quest[i].quest.Length; n++)
            {
                QuestData data = quest[i].quest[n];

                // 0~99 => current value, ongoing
                // 100 => never accepted
                // 200 => completed
                int questProgress = PLoad.Load(data.PlayerPrefs, 100);

                if (questProgress == 100)
                {
                    // 100 => never accepted
                    if (notCompleted.TryGetValue(key, out List<QuestData> value))
                    {
                        value.Add(data);
                    }
                    else
                    {
                        List<QuestData> questList = new List<QuestData> { data };
                        notCompleted.Add(quest[i].npc, questList);
                    }
                }
                else if (questProgress == 200) continue; // 200 => completed 된 퀘스트는 저장해 두지 않음
                else 
                {
                    // 0~99 => current value, ongoing
                    ongoingQuest.Add(key, CreateReferenceData(key, data, questProgress));
                }
                
            }
        }
    }

    private void RemoveCompletedQuest(NpcType _npc, QuestReferenceData _quest)
    {
        // 퀘스트를 완료하고 notCompleted에 저장된 데이터까지 지워야 다음 대화에서 다른 퀘스트를 찾을 수 있다.

        SoundManager.sInst.Play("Reward");

        _quest.QuestCompleted();

        ongoingQuest.Remove(_npc);

        QuestData targetData = _quest.GetData();

        if (notCompleted.TryGetValue(_npc, out List<QuestData> waitingQuest))
        {
            for (int i = 0; i < waitingQuest.Count; i++)
            {
                if (waitingQuest[i].RestrictedLevel <= playerLevel)
                {
                    QuestData quest = waitingQuest[i];

                    if(targetData.QuestName.Equals(quest.QuestName))
                    {
                        waitingQuest.RemoveAt(i);
                    }

                }
            }
        }
    }

    public bool FindAvailableQuestDialogue(NpcType npc, out DialogueData dialogue, out Action callback)
    {
        if(ongoingQuest.TryGetValue(npc, out QuestReferenceData activeQuest))
        {
            QuestReferenceData quest = activeQuest;

            dialogue = quest.GetCurrentDialogue();

            if(quest.IsCleared())
            {
                callback = () =>
                {
                    RemoveCompletedQuest(npc, quest);
                };
            }
            else callback = null;

            return true;
        }

        if(notCompleted.TryGetValue(npc, out List<QuestData> waitingQuest))
        {
            for (int i = 0; i < waitingQuest.Count; i++)
            {
                if(waitingQuest[i].RestrictedLevel <= playerLevel)
                {
                    QuestData quest = waitingQuest[i];

                    dialogue = quest.Dialogue[0];

                    callback = () => 
                    {
                        UIManager.inst.ShowAndGetPopup("TextLog", false)
                        .GetComponent<TextLogPopup>().UIUpdate
                        ($"[ {quest.QuestName} ] 퀘스트를 수락 하시겠습니까?", () =>
                        {
                            QuestAccepted(npc, quest);
                        });
                    };

                    return true;
                }
            }
        }

        callback = null; dialogue = null; return false;
    }

    public bool FindAvailableQuestDialogue(NpcType npc)
    {
        if (ongoingQuest.TryGetValue(npc, out QuestReferenceData activeQuest))
        {
            return true;
        }

        if (notCompleted.TryGetValue(npc, out List<QuestData> value))
        {
            for (int i = 0; i < value.Count; i++)
            {
                if (value[i].RestrictedLevel <= playerLevel) return true;
            }
        }

        return false;
    }



    public void QuestAccepted(NpcType npc, QuestData quest)
    {
        ongoingQuest.Add(npc, CreateReferenceData(npc, quest, 0));
        QuestEvent.questNotificationEvent(quest, true);
    }
    public void QuestNotification(NpcType npc, bool active)
    {
        if (npcListner.TryGetValue(npc, out INpc value))
        {
            value.QuestNotification(active);
        }
    }
    public void LevelUpEvent()
    {
        playerLevel++;
    }

    public void AddNpcListner(NpcType npc, INpc listner)
    {
        if(npcListner.TryGetValue(npc, out INpc value))
        {
            // 혹시 지워지지 않은 npc가 있다면
            npcListner.Remove(npc);
        }

        npcListner.Add(npc, listner);
    }

    public void RemoveNpcListner(NpcType npc)
    {
        if (npcListner.TryGetValue(npc, out INpc value))
        {
            npcListner.Remove(npc);
        }
    }
    private QuestReferenceData CreateReferenceData(NpcType npc, QuestData data, int currentValue)
        => new QuestReferenceData(npc, data, currentValue);
}
