using UnityEngine;
using Enums;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 스킬 트리에서 드래그하여 등록, 사용할 수 있는 스킬 퀵슬롯
/// </summary>
public class QuickSkillSlot : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] protected Image img;
    [SerializeField] protected CoolTimer cool; // 재사용 대기시간 표현
    [SerializeField] protected string playerPrefs; // 퀵슬롯 저장 정보

    protected QuickSkillSlotUIEvent eventManager; // 퀵슬롯 이벤트 매니저
    protected SkillReferenceData data;
    public virtual SkillReferenceData GetSlotData() => data;

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


    public virtual void OnDrag(PointerEventData eventData) // Drag
         => eventManager.OnDrag();
    public virtual void OnPointerDown(PointerEventData eventData) // Clicked
        => eventManager.OnPointerDown(eventData, this);
    public virtual void OnPointerUp(PointerEventData eventData) // Mouse Up
        => eventManager.OnPointerUp(eventData);

    public virtual bool IsSwappable(SkillReferenceData _slotData) // 스왑 가능한지
    {
        // 일반공격이라면 리턴
        if (_slotData != null)
        {
            if (_slotData.Type == SkillType.NormalAttack) return false;
        }

        if (IsCoolTime()) return false;

        return true;
    }

    public virtual bool IsCoolTime() // 재사용 대기시간인지 확인
    {
        return cool.IsCoolTime;
    }

    public virtual void Clone(SkillReferenceData _slotData) // 스킬북에서 스킬슬롯으로 이동
    { 

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
