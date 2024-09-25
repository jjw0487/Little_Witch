using UnityEngine;
using UnityEngine.UI;
using Enums;
using System.Collections.Generic;

public class ShortcutPopup : IPopup
{
    [SerializeField] private Button btn_Exit;
    [SerializeField] private DragText dragText;
    [SerializeField] private ShortcutSlot[] swappableSlot;

    private ShortcutSlotUIEvent slotEventManager;

    private Dictionary<KeyCode, ShortcutSlot> slotDic = new Dictionary<KeyCode, ShortcutSlot>();

    private List<KeyEvent> keyEvents;


    public void Init(List<KeyEvent> _keyEvents)
    {
        keyEvents = _keyEvents;

        slotEventManager = new ShortcutSlotUIEvent(dragText);

        for (int i = 0; i < swappableSlot.Length; i++)
        {
            swappableSlot[i].Init(slotEventManager);

            if (slotDic.TryGetValue(swappableSlot[i].GetKeyCode(), out ShortcutSlot slot)) continue;

            slotDic.Add(swappableSlot[i].GetKeyCode(), swappableSlot[i]);

        }

        LoadShortcutSlot();

        btn_Exit.onClick.AddListener(Exit);

        SlideIn();
    }

    private void LoadShortcutSlot()
    {
        this.transform.SetAsLastSibling();

        for (int i = 0; i < keyEvents.Count; i++) 
        {
            if(slotDic.TryGetValue(keyEvents[i].GetKeyCode(), out ShortcutSlot slot))
            {
                slot.Swap(keyEvents[i]);
            }
            else
            {
                Debug.Log(keyEvents[i].GetEventName() + " 슬롯 못찾음");
            }
        }
    }

    public override void UIUpdate()
    {
        this.transform.SetAsLastSibling();

        LoadShortcutSlot();
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

        SlideOut(() =>
        {
            tr_Body.localPosition = Vector2.zero;
            this.transform.gameObject.SetActive(false);
        });
    }

}
