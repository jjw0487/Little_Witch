using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class QuickSlotManager : MonoBehaviour
{
    /// <summary>
    /// 퀵 아이템 슬롯과 퀵 스킬 슬롯을 모두 관장한다.
    /// </summary>

    [SerializeField] private QuickItemSlot[] quickItemSlots;
    [SerializeField] private QuickSkillSlot[] quickSkillSlots;
    [SerializeField] private DragImage dragImage;
    
    private QuickItemSlotUIEvent quickItemSlotEvent;
    private QuickSkillSlotUIEvent quickSkillSlotEvent;

    public void InitializeQuickSlotManager()
    {
        quickItemSlotEvent = new QuickItemSlotUIEvent(dragImage);

        for(int i =0;i< quickItemSlots.Length;i++) 
        {
            quickItemSlots[i].InitializeQuickItemSlot(quickItemSlotEvent);
        }

        quickSkillSlotEvent = new QuickSkillSlotUIEvent(dragImage);

        for (int i = 0; i < quickSkillSlots.Length; i++)
        {
            quickSkillSlots[i].InitializeQuickSkillSlot(quickSkillSlotEvent);
        }
    }

    public void InventoryKeyEvent()
    {
        UIManager.inst.ShowPopup("Inventory", true);
    }

    public void QuestKeyEvent()
    {
        UIManager.inst.ShowPopup("Quest", true);
    }

    public void SkillTreeKeyEvent()
    {
        UIManager.inst.ShowPopup("SkillTree", true);
    }

    public void SettingKeyEvent()
    {
        UIManager.inst.ShowPopup("Setting", true);
    }


}
