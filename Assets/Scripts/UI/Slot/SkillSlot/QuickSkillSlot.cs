using UnityEngine;
using Enums;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// ��ų Ʈ������ �巡���Ͽ� ���, ����� �� �ִ� ��ų ������
/// </summary>
public class QuickSkillSlot : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] protected Image img;
    [SerializeField] protected CoolTimer cool; // ���� ���ð� ǥ��
    [SerializeField] protected string playerPrefs; // ������ ���� ����

    protected QuickSkillSlotUIEvent eventManager; // ������ �̺�Ʈ �Ŵ���
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

    public virtual bool IsSwappable(SkillReferenceData _slotData) // ���� ��������
    {
        // �Ϲݰ����̶�� ����
        if (_slotData != null)
        {
            if (_slotData.Type == SkillType.NormalAttack) return false;
        }

        if (IsCoolTime()) return false;

        return true;
    }

    public virtual bool IsCoolTime() // ���� ���ð����� Ȯ��
    {
        return cool.IsCoolTime;
    }

    public virtual void Clone(SkillReferenceData _slotData) // ��ų�Ͽ��� ��ų�������� �̵�
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
        // ��Ÿ���� ������ �� �÷��̾ ��ų�� ����� �� �ִ��� ���� üũ�Ѵ�
        // ( �̹� ��ų ���� ��, MP�� ��ų�� ����� ��ŭ �ִ���... �� )

        if (data == null) return;

        if (IsCoolTime()) return;

        PlayerEvent.skillEvent(data, () =>
        {
            // ��ų ��� ���� �� callback
            cool.Init(data.CoolTime);
        });
    }

}
