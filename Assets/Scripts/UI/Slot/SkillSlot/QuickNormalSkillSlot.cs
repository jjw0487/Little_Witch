using Enums;
using UnityEngine;
using UnityEngine.UI;

public class QuickNormalSkillSlot : QuickSkillSlot
{
    [SerializeField] private Text txt_Stack;
    [SerializeField] private float stackCoolTime = 3f;

    private int stacker = 0;
    private float count;

    private void OnEnable()
    {
        EventManager.normalAttackEvent += UseSkill;
    }
    private void OnDisable()
    {
        EventManager.normalAttackEvent -= UseSkill;
    }

    public override void InitializeQuickSkillSlot(QuickSkillSlotUIEvent _eventManager)
    {
        eventManager = _eventManager;

        count = stackCoolTime;

        int savedSkill = PLoad.Load(playerPrefs, 0);

        SkillReferenceData _data =
                PlayManager.inst.Skill().GetSkillData((SkillKey)savedSkill);

        //Debug.Log($"skill key : {(SkillKey)savedSkill}, data : {_data == null}");

        if (_data != null)
        {
            Clone(_data);
            stacker = 5;
            txt_Stack.text = stacker.ToString();
        }
        else
        {
            stacker = 6;
            txt_Stack.text = "";
        }
    }

    private void FixedUpdate()
    {
        if(stacker < 5)
        {
            count -= Time.deltaTime;

            if(count <= 0)
            {
                StackUpdate(1);
                count = stackCoolTime;
            }
        }
    }
    public override void RemoveData()
    {
        if (data != null)
        {
            eventManager.RemoveSkillList(data);
        }

        data = null;
        img.gameObject.SetActive(false);

        stacker = 6;
        txt_Stack.text = "";

        PSave.Save(playerPrefs, 0);
    }
    private void StackUpdate(int value)
    {
        stacker += value;

        if (stacker > 5) stacker = 5;

        txt_Stack.text = stacker.ToString();
    }

    public override void Clone(SkillReferenceData _slotData)
    { // ��ų�Ͽ��� ��ų�������� �̵�

        if (IsCoolTime()) return;

        if (eventManager.IsSkillExistInList(_slotData)) return;

        // ���� �����Ͱ� Ŭ�еǼ� ����Ŀ�� �ʱ�ȭ�Ǹ� �ȵ�.
        if (data == _slotData) return;

        if (data != null)
        {
            eventManager.RemoveSkillList(data);
        }

        data = _slotData;

        if (!IsNull())
        {
            img.sprite = data.Sprt;
            img.gameObject.SetActive(true);
            stacker = 0;
            txt_Stack.text = stacker.ToString();
            eventManager.AddSkillList(data);

            PSave.Save(playerPrefs, (int)data.Key);
        }
        else
        {
            PSave.Save(playerPrefs, 0);
        }

        
    }

    public override bool IsSwappable(SkillReferenceData _slotData)
    {
        // �Ϲݰ����� �ƴ϶�� ����
        if (_slotData != null)
        {
            if(_slotData.Type != SkillType.NormalAttack) return false;
        }
            

        if (IsCoolTime()) return false;

        return true;
    }

    public override void UseSkill()
    {
        // �븻��ų�� �� �� �ٸ�

        // ��Ÿ���� ������ �� �÷��̾ ��ų�� ����� �� �ִ��� ���� üũ�Ѵ�

        if (IsCoolTime()) return;

        if (stacker == 0) return;

        if (!PlayerEvent.skillEvent(data, () => 
        {
            // ��ų ��� ���� �� callback
            cool.Init(data.CoolTime);
            StackUpdate(-1);

        })) return;

        

    }
}
