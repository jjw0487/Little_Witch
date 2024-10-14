using Enums;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShortcutSlot : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private Image img_Frame;
    [SerializeField] private Text txt_EventName;
    [SerializeField] private KeyCode code;

    private ShortcutSlotUIEvent eventManager;
    private KeyEvent data;
    private bool isEmpty = true;

    public KeyEvent GetData() => data;
    public KeyCode GetKeyCode() => code;
    public bool IsSwappable() => true;
    public bool IsEmpty() => isEmpty;

    public void Init(ShortcutSlotUIEvent _eventManager)
    {
        this.eventManager = _eventManager;
    }

    public void Swap(KeyEvent _data)
    {
        data = _data;
        isEmpty = data == null;

        if (isEmpty)
        {
            img_Frame.color = new Color(0.69f, 0.85f, 0.94f);
            txt_EventName.text = "";
        }
        else
        {
            data.ChangeKeyCode(code);
            img_Frame.color = new Color(0.98f, 0.78f, 0.37f);
            txt_EventName.text = data.GetEventName();
        }
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!IsEmpty()) eventManager.OnPointerDown(this);
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!IsEmpty()) eventManager.OnDrag();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!IsEmpty()) eventManager.OnPointerUp(eventData);
    }
}
