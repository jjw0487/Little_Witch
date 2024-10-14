using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestCompleteNotification : QuestNotification
{
    [SerializeField] protected Text txt_QuestName;
    public override void Spawn(QuestData quest)
    {
        isOn = true;

        txt_QuestName.text = $"[{quest.QuestName}] Quest Completed !";

        base.Spawn(quest);
    }
}
