using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��ų �˾� ( ��ų ������, ��ų ������ �� ��ȣ�ۿ� )
/// ��ų ��带 ���� ����� ��ų, ����Ǿ�� �� ��ų���� UI�� ǥ���Ѵ�.
/// </summary>
public class SkillTreePopup : IPopup
{
    [SerializeField] private Button btn_Exit;
    [SerializeField] private DragImage dragImg;
    [SerializeField] private Text txt_SkillPoint;
    [SerializeField] private SkillNode firstNode; // ù��° ��带 ������Ʈ �ϸ� ����� ������ ���鵵 �˾Ƽ� ������Ʈ �ȴ�
    [SerializeField] private SkillTreeDescriptionPanel descPanel;
    [SerializeField] private SkillPanelSlot[] slots;

    private SkillTreeUIEvent slotEventManager; // ������, ��ų ������, ��ų ���� UI �� �̺�Ʈ�� ����
    //

    private void OnEnable()
    {
        EventManager.uiUpdateEvent += SkillPointUpdate; // ��ų ����Ʈ�� ����Ǹ� UI�� ��� ������Ʈ
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

        // ù��° ��带 ������Ʈ �ϸ� ����� �������� �ʿ� �۾��� �����Ѵ�.
        firstNode.NodeUpdate(DataContainer.sInst.PlayerStatus().Level, 
            PlayManager.inst.Skill().SkillPoint > 0, true);
    }

    public override void UIUpdate()
    {
        this.transform.SetAsLastSibling();

        SkillPointUpdate();
        // �����
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
