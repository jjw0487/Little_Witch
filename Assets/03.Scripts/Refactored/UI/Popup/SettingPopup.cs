using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

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

    private void ShortcutPopup()
    {
        OnButtonClick();
        UIManager.inst.ShortcutPopup();
    }

    private void GameQuit()
    {
        OnButtonClick();
        Application.Quit();
    }

    public override void EscapePressed()
    {
        Exit();
    }

    public override void Exit()
    {
        if (isTweening) return;

        SlideOut(() => {

            tr_Body.localPosition = Vector2.zero;

            this.transform.gameObject.SetActive(false);
        });
    }
}
