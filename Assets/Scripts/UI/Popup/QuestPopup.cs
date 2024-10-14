using Enums;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ����Ʈ ���൵�� Ȯ���� �� �ִ� �˾�
/// </summary>
public class QuestPopup : IPopup
{
    [SerializeField] private RectTransform content; // ��ũ��

    [SerializeField] private Button btn_Exit; 
    [SerializeField] private QuestPanelUnit obj_Unit;
    [SerializeField] private List<QuestPanelUnit> units;
    
    private Dictionary<NpcType, QuestReferenceData> ongoingQuest; // �������� ����Ʈ ������

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        ongoingQuest = DataContainer.sInst.Quest().GetOngoingQuest(); // �������� ����Ʈ ������

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
        var obj = GameObject.Instantiate(obj_Unit, content); // ��ũ�ѿ� ����� �������� ����Ʈ ����
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
