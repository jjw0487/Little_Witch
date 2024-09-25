using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleSceneUIManager : UIManager
{
    [SerializeField] private AudioClip titleSceneBGM;

    [SerializeField] private Transform buttonUis;
    [SerializeField] private Button btn_NewGame;
    [SerializeField] private Button btn_ContinueGame;
    [SerializeField] private Button btn_GameSetting;

    private void Start()
    {
        InitializeGameSetting();

        InitializeUIManager(null);
    }

    private void InitializeGameSetting()
    {
        Screen.SetResolution(PLoad.Load("Resolution_Width", 1920), PLoad.Load("Resolution_Height", 1080), Screen.fullScreen);

        QualitySettings.vSyncCount = 0;

        int fps = PLoad.Load("TargetFrameRate", 144);

        Application.targetFrameRate = fps;

        SoundManager.sInst.InitializeSoundManager();

        SoundManager.sInst.PlayBGM(titleSceneBGM);
    }



    public override void InitializeUIManager(PlayerController player)
    {
        base.InitializeUIManager(player);

        bool isNewGame = PLoad.Load("IsNewGame", true);

        btn_NewGame.onClick.AddListener(NewGame);

        btn_ContinueGame.onClick.AddListener(() => {
            DataContainer.sInst.InitializeDataContainer();
            SceneLoader.sInst.LoadScene("02.TownScene"); 
        });

        btn_ContinueGame.gameObject.SetActive(!isNewGame);

        btn_GameSetting.onClick.AddListener(() => { ShowPopup("Setting", true); });
    }

    private void NewGame()
    {
        PlayerPrefs.DeleteAll();

        DataContainer.sInst.InitializeDataContainer();

        SceneLoader.sInst.LoadScene("02.TownScene");
    }


    public override void HideGUI()
    {
        buttonUis.gameObject.SetActive(false);
    }

    public override void ShowGUI() { }
    public override void StartConversation(DialogueData dialogue, Action _callback = null) { }
    public override void EndConversation(){}
    public override void ShortcutPopup()
    {
    }
    protected override void Update()
    {
        base.Update();
    }
    protected override void ESCPressed()
        => popupController.EscapePressed();
    public override GameObject GetDisposablePopup(string popup)
        => popupController.GetDisposablePopup(popup);
    public override void ShowPopup(string popupName, bool stack)
        => popupController.ShowPopup(popupName, stack);
    public override GameObject ShowAndGetPopup(string popupName, bool stack)
        => popupController.ShowAndGetPopup(popupName, stack);

    
}
