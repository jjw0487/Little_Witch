using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 타이틀 씬의 Setting이나 단축키를 통해 SettingPopup 을 호출할 수 있다.
/// </summary>
public class SettingPopup : IPopup
{
    [SerializeField] private Button btn_Exit;
    [SerializeField] private Button btn_Shortcut;
    [SerializeField] private Button btn_GameQuit;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        btn_Exit.onClick.AddListener(Exit);
        btn_Shortcut.onClick.AddListener(ShortcutPopup);
        btn_GameQuit.onClick.AddListener(GameQuit);

        UIUpdate();
    }


    public override void UIUpdate()
    {
        this.transform.SetAsLastSibling();

        SlideIn();
    }

    private void ShortcutPopup() // 숏컷(단축키) 설정
    {
        OnButtonClick();
        UIManager.inst.ShortcutPopup();
    }

    private void GameQuit() // 게임종료
    {
        OnButtonClick();
        Application.Quit();
    }

    public override void EscapePressed() // Keycode.Escape 호출되었을 시
    {
        Exit();
    }

    public override void Exit() // 팝업 종료
    {
        if (isTweening) return;

        SlideOut(() => {

            tr_Body.localPosition = Vector2.zero;

            this.transform.gameObject.SetActive(false);
        });
    }
}
