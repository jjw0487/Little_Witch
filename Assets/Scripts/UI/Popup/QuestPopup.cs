using Enums;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 퀘스트 진행도를 확인할 수 있는 팝업
/// </summary>
public class QuestPopup : IPopup
{
    [SerializeField] private RectTransform content; // 스크롤

    [SerializeField] private Button btn_Exit; 
    [SerializeField] private QuestPanelUnit obj_Unit;
    [SerializeField] private List<QuestPanelUnit> units;
    
    private Dictionary<NpcType, QuestReferenceData> ongoingQuest; // 진행중인 퀘스트 데이터

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        ongoingQuest = DataContainer.sInst.Quest().GetOngoingQuest(); // 진행중인 퀘스트 데이터

        btn_Exit.onClick.AddListener(Exit); 

        UIUpdate();
    }


    public override void UIUpdate()
    {
        this.transform.SetAsLastSibling();

        int initializedPanelCount = 0;

        foreach (KeyValuePair<NpcType, QuestReferenceData> quests in ongoingQuest)
        {
            if (initializedPanelCount >= units.Count)
            {
                CreateAndAddPanel();
            }

            units[initializedPanelCount].Init(quests.Value);

            initializedPanelCount++;
        }

        Vector2 panelViewSize = new Vector2(472f, initializedPanelCount * 207f + 14f);

        content.sizeDelta = panelViewSize;

        SlideIn();
    }

    private void CreateAndAddPanel()
    {
        var obj = GameObject.Instantiate(obj_Unit, content); // 스크롤에 노출될 진행중인 퀘스트 유닛
        units.Add(obj);
    }

    public override void EscapePressed()
    {
        Exit();
    }

    public override void Exit()
    {
        if (isTweening) return;

        SlideOut(() => {
            tr_Body.localPosition = Vector2.zero;

            for(int i = 0; i < units.Count; i++)
            {
                units[i].DisablePanle();
            }

            this.transform.gameObject.SetActive(false);
        });
    }
}
