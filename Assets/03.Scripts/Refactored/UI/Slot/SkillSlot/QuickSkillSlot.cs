using UnityEngine;
using Enums;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuickSkillSlot : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    protected QuickSkillSlotUIEvent eventManager;

    [SerializeField] protected Image img;

    [SerializeField] protected CoolTimer cool;

    [SerializeField] protected string playerPrefs;

    protected SkillReferenceData data;

    public virtual void InitializeQuickSkillSlot(QuickSkillSlotUIEvent _eventManager)
    {
        eventManager = _eventManager;

        int savedSkill = PLoad.Load(playerPrefs, 0);

        if (savedSkill != 0) 
        {
            SkillReferenceData _data =
                PlayManager.inst.Skill().GetSkillData((SkillKey)savedSkill);

            if (_data != null)
            {
                Clone(_data);
            }
        }
    }


    public virtual void OnDrag(PointerEventData eventData)
         => eventManager.OnDrag();
    public virtual void OnPointerDown(PointerEventData eventData)
        => eventManager.OnPointerDown(eventData, this);
    public virtual void OnPointerUp(PointerEventData eventData)
        => eventManager.OnPointerUp(eventData);

    public virtual bool IsSwappable(SkillReferenceData _slotData)
    {
        // 일반공격이라면 리턴
        if (_slotData != null)
        {
            if (_slotData.Type == SkillType.NormalAttack) return false;
        }

        if (IsCoolTime()) return false;

        return true;
    }

    public virtual bool IsCoolTime()
    {
        return cool.IsCoolTime;
    }

    public virtual SkillReferenceData GetSlotData() => data;

    public virtual void Clone(SkillReferenceData _slotData)
    { // 스킬북에서 스킬슬롯으로 이동

        if (IsCoolTime()) return;

        if (eventManager.IsSkillExistInList(_slotData)) return;

        if (data != null)
        {
            eventManager.RemoveSkillList(data);
        }

        data = _slotData;

        if (!IsNull())
        {
            img.sprite = data.Sprt;
            img.gameObject.SetActive(true);
            eventManager.AddSkillList(data);

            PSave.Save(playerPrefs, (int)data.Key);
        }
        else
        {
            PSave.Save(playerPrefs, 0);
        }
    }

    public virtual void RemoveData()
    {
        if(data != null)
        {
            eventManager.RemoveSkillList(data);
        }

        data = null;
        img.gameObject.SetActive(false);

        PSave.Save(playerPrefs, 0);
    }

    public virtual void UIUpdate()
    {
        if (!IsNull())
        {
            img.sprite = data.Sprt;
            img.gameObject.SetActive(true);
        }
    }

    public virtual Sprite GetSprite() => data.Sprt;
    public virtual bool IsEmpty() => data == null;

    protected virtual bool IsNull()
    {
        if (data == null)
        {
            img.gameObject.SetActive(false);
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual void UseSkill()
    {
        // 쿨타임을 돌리기 전 플레이어가 스킬을 사용할 수 있는지 먼저 체크한다
        // ( 이미 스킬 시전 중, MP가 스킬을 사용할 만큼 있는지... 등 )

        if (data == null) return;

        if (IsCoolTime()) return;

        PlayerEvent.skillEvent(data, () =>
        {
            // 스킬 사용 성공 시 callback
            cool.Init(data.CoolTime);
        });
    }

}
