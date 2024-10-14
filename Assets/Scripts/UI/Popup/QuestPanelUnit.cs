using UnityEngine;
using UnityEngine.UI;

public class QuestPanelUnit : MonoBehaviour
{
    [SerializeField] private Text txt_QuestName;
    [SerializeField] private Text txt_Desc;
    [SerializeField] private Text txt_Npc;
    [SerializeField] private Text txt_Progress;
    [SerializeField] private Text txt_State;
    [SerializeField] private Image img_Quest;
    [SerializeField] private Image img_State;

    [SerializeField] private Color color_State_Ongoing;
    [SerializeField] private Color color_State_Completed;

    public void Init(QuestReferenceData data)
    {
        QuestData quest = data.GetData();

        img_Quest.sprite = quest.Sprite;

        txt_QuestName.text = quest.QuestName;
        txt_Desc.text = quest.Contents;
        txt_Npc.text = $"NPC : {quest.Npc}";
        txt_Progress.text = $"{data.GetCurrentValue()} / {quest.GoalValue}";
        txt_State.text = data.IsCleared() ? "완료" : "진행중";
        img_State.color = data.IsCleared() ? color_State_Completed : color_State_Ongoing;

        this.gameObject.SetActive(true);
    }

    public void DisablePanle()
    {
        this.gameObject.SetActive(false);
    }
}
