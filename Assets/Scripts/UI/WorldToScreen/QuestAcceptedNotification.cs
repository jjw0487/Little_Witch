using UnityEngine;
using UnityEngine.UI;

public class QuestAcceptedNotification : QuestNotification
{
    [SerializeField] protected Image img_Quest;
    [SerializeField] protected Text txt_QuestName;
    [SerializeField] protected Text txt_Desc;
    public override void Spawn(QuestData quest)
    {
        isOn = true;

        img_Quest.sprite = quest.Sprite;
        txt_QuestName.text = $"[{quest.QuestName}] Quest Accepted !";
        //txt_Value.text = "0 / " + quest.GoalValue;
        txt_Desc.text = quest.Contents;

        base.Spawn(quest);
    }
}
