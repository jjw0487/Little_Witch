using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 타이틀 씬 화면의 버튼, 팝업을 관리
/// </summary>
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
        // 초기 실행 시 설정된 해상도가 없다면 1920x1080 으로 설정

        QualitySettings.vSyncCount = 0;

        int fps = PLoad.Load("TargetFrameRate", 144);

        Application.targetFrameRate = fps;
        // 초기 실행 시 설정된 프레임 수치가 없다면 144로 설정

        SoundManager.sInst.InitializeSoundManager();
        // 사운드 매니저 초기화

        SoundManager.sInst.PlayBGM(titleSceneBGM);
        // 씬 마다 설정된 배경음 실행
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
        PlayerPrefs.DeleteAll(); // 새로운 게임을 실행시 PlayerPrefs 데이터 제거 후 실행

        DataContainer.sInst.InitializeDataContainer(); // 저장 데이터 초기화

        SceneLoader.sInst.LoadScene("02.TownScene"); // 씬 이동
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
