using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class KeyEvent : MonoBehaviour
{
    [SerializeField] private Text keyText;
    [SerializeField] private KeyCode keyCode;
    [SerializeField] private string eventName;
    [SerializeField] private UnityEvent keyEvent;

    private string playerprefs;

    public KeyCode GetKeyCode() => keyCode;
    public string GetEventName() => eventName;

    public void Initialize(string _playerprefs)
    {
        playerprefs = _playerprefs;
        keyCode = (KeyCode)PLoad.Load(playerprefs, (int)keyCode);

        if (keyText != null)
        {
            keyText.text = ConvertTo.KeycodeToString(keyCode);
        }
    }

    public void ChangeKeyCode(KeyCode _keyCode)
    {
        keyCode = _keyCode;

        if (keyText != null)
        {
            keyText.text = ConvertTo.KeycodeToString(keyCode);
        }

        PSave.Save(playerprefs, (int)keyCode);
    }

    public void InputEvent()
    {
        if(Input.GetKeyDown(keyCode))
        {
            keyEvent.Invoke();
        }
    }
}
