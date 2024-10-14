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
    { // 스킬북에서 스킬슬롯으로 이동

        if (IsCoolTime()) return;

        if (eventManager.IsSkillExistInList(_slotData)) return;

        // 같은 데이터가 클론되서 스태커가 초기화되면 안됨.
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
        // 일반공격이 아니라면 리턴
        if (_slotData != null)
        {
            if(_slotData.Type != SkillType.NormalAttack) return false;
        }
            

        if (IsCoolTime()) return false;

        return true;
    }

    public override void UseSkill()
    {
        // 노말스킬은 좀 더 다름

        // 쿨타임을 돌리기 전 플레이어가 스킬을 사용할 수 있는지 먼저 체크한다

        if (IsCoolTime()) return;

        if (stacker == 0) return;

        if (!PlayerEvent.skillEvent(data, () => 
        {
            // 스킬 사용 성공 시 callback
            cool.Init(data.CoolTime);
            StackUpdate(-1);

        })) return;

        

    }
}
