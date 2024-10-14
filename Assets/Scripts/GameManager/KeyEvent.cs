using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// 단축키 이벤트
/// </summary>
public class KeyEvent : MonoBehaviour
{
    [SerializeField] private Text keyText; // UI에 표현될 필요가 있을 시
    [SerializeField] private KeyCode keyCode; // 실행 키 코드
    [SerializeField] private string eventName; // 팝업에 표시될 이벤트 이름
    [SerializeField] private UnityEvent keyEvent; // 이벤트를 담아 놓음

    private string playerprefs;

    public KeyCode GetKeyCode() => keyCode;
    public string GetEventName() => eventName;

    public void Initialize(string _playerprefs)
    {
        playerprefs = _playerprefs;

        keyCode = (KeyCode)PLoad.Load(playerprefs, (int)keyCode); // 저장된 키코드를 불러옴

        if (keyText != null)
        {
            keyText.text = ConvertTo.KeycodeToString(keyCode);
        }
    }

    public void ChangeKeyCode(KeyCode _keyCode) // 단축키 변경 이벤트 발생 시
    {
        keyCode = _keyCode;

        if (keyText != null)
        {
            keyText.text = ConvertTo.KeycodeToString(keyCode);
        }

        PSave.Save(playerprefs, (int)keyCode);
    }

    public void InputEvent() // 인풋 이벤트를 실행함
    {
        if(Input.GetKeyDown(keyCode))
        {
            keyEvent.Invoke();
        }
    }
}
