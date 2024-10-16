using System;
using UnityEngine;

/// <summary>
/// ���� NPC
/// </summary>
public class Aranara : Npc
{
    private bool isFirstMeeting;
    private void OnDisable()
    {
        if (DataContainer.sInst != null) DataContainer.sInst.Quest().RemoveNpcListner(type);
    }

    private void Start()
    {
        InitializeNpc();
    }

    public override void InitializeNpc()
    {
        isFirstMeeting = PLoad.Load(playerPrefs, true);
        if (DataContainer.sInst != null) DataContainer.sInst.Quest().AddNpcListner(type, this);

        if(!isFirstMeeting)
        {
            if(DataContainer.sInst.Quest().
                FindAvailableQuestDialogue(type))
            {
                QuestNotification(true);
            }

        }
    }

    public override void QuestNotification(bool active)
    {
        if (notification != null) 
        {
            notification.gameObject.SetActive(active);
        }
    }

    public override void StartConversation(Transform target)
    {
        QuestNotification(false);

        if (isFirstMeeting)
        {
            PlayManager.inst.Interact().StartConversation(IdleDialogue[0], this.transform, () => 
            {
                if (DataContainer.sInst.Quest().
                FindAvailableQuestDialogue(type))
                {
                    QuestNotification(true);
                }
            });

            PSave.Save(playerPrefs, false);

            isFirstMeeting = false;
        }
        else
        {
            if (DataContainer.sInst.Quest().
            FindAvailableQuestDialogue(type, out DialogueData data, out Action callback))
            {
                PlayManager.inst.Interact().StartConversation(data, this.transform, () =>
                {
                    if (callback != null)
                    {
                        callback();
                    }
                });
            }
            else
            {
                PlayManager.inst.Interact().StartConversation(IdleDialogue[1], this.transform);
            }
        }

        Quaternion dir = Quaternion.LookRotation((target.position - this.transform.position).normalized);
        transform.rotation = Quaternion.Euler(0f, dir.eulerAngles.y, 0f);
    }
}
