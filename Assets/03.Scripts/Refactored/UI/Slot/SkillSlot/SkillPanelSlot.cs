using Enums;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 스킬 포인트로 스킬을 레벨업 하고 그 정보를 저장하는 곳
/// </summary>
public class SkillPanelSlot : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject skillLock;
    [SerializeField] private SkillKey key;
    [SerializeField] private Button btn_LevelUp;
    [SerializeField] private Text txt_SkillLevel;

    private SkillReferenceData data;
    private bool isInitialized = false;
    private SkillTreeUIEvent eventManager;
    public bool IsSkillAvailable() => data.Level > 0;
    public void Initialize(SkillTreeUIEvent _eventManager)
    {
        isInitialized = false;
        eventManager = _eventManager;
        data = PlayManager.inst.Skill().GetSkillData(key);
        btn_LevelUp.onClick.AddListener(LevelUp);
        txt_SkillLevel.text = data.Level.ToString();
        isInitialized = true;
    }

    public void LockEvent(bool isUnlocked, bool isPointAvailable)
    {
        if(isUnlocked)
        {
            skillLock.SetActive(false);

            btn_LevelUp.gameObject.SetActive(isPointAvailable);

            if (data.IsMaxLevel()) btn_LevelUp.gameObject.SetActive(false);
        }
        else
        {
            skillLock.SetActive(true);

            btn_LevelUp.gameObject.SetActive(false);
        }

    }
    
    
    private void LevelUp()
    {
        if (isInitialized)
        {
            if(data.LevelUp())
            {
                SoundManager.sInst.Play("ButtonClick");

                PlayManager.inst.Skill().DecreaseSkillPoint();
                txt_SkillLevel.text = data.Level.ToString();
            }

            if(data.IsMaxLevel()) btn_LevelUp.gameObject.SetActive(false);
        }
    }

    public Sprite GetSprite()
    {
        return data.Sprt;
    }

    public SkillReferenceData GetSkillData()
    {
        return data;
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (!IsSkillAvailable()) return;

        eventManager.OnDrag();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!IsSkillAvailable()) return;

        eventManager.OnPointerDown(this);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!IsSkillAvailable()) return;

        eventManager.OnPointerUp(eventData);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        eventManager.OnMouseEnter(data.Data, this.transform.position);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        eventManager.OnMouseExit();
    }
}
