using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// ����Ű �̺�Ʈ
/// </summary>
public class KeyEvent : MonoBehaviour
{
    [SerializeField] private Text keyText; // UI�� ǥ���� �ʿ䰡 ���� ��
    [SerializeField] private KeyCode keyCode; // ���� Ű �ڵ�
    [SerializeField] private string eventName; // �˾��� ǥ�õ� �̺�Ʈ �̸�
    [SerializeField] private UnityEvent keyEvent; // �̺�Ʈ�� ��� ����

    private string playerprefs;

    public KeyCode GetKeyCode() => keyCode;
    public string GetEventName() => eventName;

    public void Initialize(string _playerprefs)
    {
        playerprefs = _playerprefs;

        keyCode = (KeyCode)PLoad.Load(playerprefs, (int)keyCode); // ����� Ű�ڵ带 �ҷ���

        if (keyText != null)
        {
            keyText.text = ConvertTo.KeycodeToString(keyCode);
        }
    }

    public void ChangeKeyCode(KeyCode _keyCode) // ����Ű ���� �̺�Ʈ �߻� ��
    {
        keyCode = _keyCode;

        if (keyText != null)
        {
            keyText.text = ConvertTo.KeycodeToString(keyCode);
        }

        PSave.Save(playerprefs, (int)keyCode);
    }

    public void InputEvent() // ��ǲ �̺�Ʈ�� ������
    {
        if(Input.GetKeyDown(keyCode))
        {
            keyEvent.Invoke();
        }
    }
}
