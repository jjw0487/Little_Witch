using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 스킬 팝업 ( 스킬 레벨업, 스킬 퀵슬롯 과 상호작용 )
/// 스킬 노드를 따라 연결된 스킬, 선행되어야 할 스킬들을 UI로 표시한다.
/// </summary>
public class SkillTreePopup : IPopup
{
    [SerializeField] private Button btn_Exit;
    [SerializeField] private DragImage dragImg;
    [SerializeField] private Text txt_SkillPoint;
    [SerializeField] private SkillNode firstNode; // 첫번째 노드를 업데이트 하면 연결된 나머지 노드들도 알아서 업데이트 된다
    [SerializeField] private SkillTreeDescriptionPanel descPanel;
    [SerializeField] private SkillPanelSlot[] slots;

    private SkillTreeUIEvent slotEventManager; // 퀵슬롯, 스킬 레벨업, 스킬 정보 UI 등 이벤트를 관리
    //

    private void OnEnable()
    {
        EventManager.uiUpdateEvent += SkillPointUpdate; // 스킬 포인트가 변경되면 UI를 즉시 업데이트
    }

    private void OnDisable()
    {
        EventManager.uiUpdateEvent -= SkillPointUpdate;
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        slotEventManager = new SkillTreeUIEvent(dragImg, descPanel);

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Initialize(slotEventManager);
        }

        btn_Exit.onClick.AddListener(Exit);

        UIUpdate();
    }

    private void SkillPointUpdate()
    {
        txt_SkillPoint.text = PlayManager.inst.Skill().SkillPoint.ToString();

        // 첫번째 노드를 업데이트 하면 연결된 나머지도 필요 작업을 진행한다.
        firstNode.NodeUpdate(DataContainer.sInst.PlayerStatus().Level, 
            PlayManager.inst.Skill().SkillPoint > 0, true);
    }

    public override void UIUpdate()
    {
        this.transform.SetAsLastSibling();

        SkillPointUpdate();
        // 재실행
        SlideIn();
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
            this.transform.gameObject.SetActive(false);
        });

        
    }
}
