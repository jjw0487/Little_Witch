using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreePopup : IPopup
{
    [SerializeField] private Button btn_Exit;
    [SerializeField] private DragImage dragImg;
    [SerializeField] private Text txt_SkillPoint;
    [SerializeField] private SkillNode firstNode; // 첫번째 노드를 업데이트 하면 연결된 노드들이 알아서 업데이트 한다
    [SerializeField] private SkillTreeDescriptionPanel descPanel;
    [SerializeField] private SkillPanelSlot[] slots;

    private SkillTreeUIEvent slotEventManager;
    //

    private void OnEnable()
    {
        EventManager.uiUpdateEvent += SkillPointUpdate;
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
