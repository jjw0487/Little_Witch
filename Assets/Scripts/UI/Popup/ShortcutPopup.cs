using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 단축키 설정을 위한 팝업
/// </summary>
public class ShortcutPopup : IPopup
{
    [SerializeField] private Button btn_Exit;
    [SerializeField] private DragText dragText;
    [SerializeField] private ShortcutSlot[] swappableSlot;

    private ShortcutSlotUIEvent slotEventManager;
    private Dictionary<KeyCode, ShortcutSlot> slotDic = new Dictionary<KeyCode, ShortcutSlot>();

    private List<KeyEvent> keyEvents; // 해당 씬에서 필요한 단축키들
    public void Init(List<KeyEvent> _keyEvents)
    {
        keyEvents = _keyEvents; // 팝업 실행 시 키 이벤트 리스트를 받음

        // 슬롯 이벤트 (드래그, 드랍)
        slotEventManager = new ShortcutSlotUIEvent(dragText);

        for (int i = 0; i < swappableSlot.Length; i++)
        {
            swappableSlot[i].Init(slotEventManager);

            if (slotDic.TryGetValue(swappableSlot[i].GetKeyCode(), out ShortcutSlot slot)) continue;

            slotDic.Add(swappableSlot[i].GetKeyCode(), swappableSlot[i]);

        }

        LoadShortcutSlot(); // 저장된 키코드에 맞게 팝업에 배치

        btn_Exit.onClick.AddListener(Exit);

        SlideIn();
    }

    private void LoadShortcutSlot() // 저장된 키코드에 맞게 팝업에 배치
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
