using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonScenePlayManager : PlayManager
{
    [SerializeField] private AudioClip bgm;

    [SerializeField] private PlayerController player;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private SkillManager skillManager;

    [SerializeField] private CameraMovement followCamera;
    [SerializeField] private MinimapCamera minimapCamera;

    [SerializeField] private Transform spawnPoint;

    private PlayerStatusData statData;
    private InteractionManager interationManager;

    public override PlayerController GetPlayer() => player;
    public override int GetPlayerLevel() => statData.Level;
    public override InteractionManager Interact() => interationManager;
    public override SkillManager Skill() => skillManager;

    void Start()
    {
        InitializeTownScene();
    }

    private void InitializeTownScene()
    {
        statData = DataContainer.sInst.PlayerStatus();

        skillManager.InitializeSkillManager(statData);

        player.InitializePlayer(spawnPoint, followCamera, statData);

        uiManager.InitializeUIManager(player);

        followCamera.InitializeFollowCamera(player, Enums.PlayModeType.Street, spawnPoint.position);

        minimapCamera.InitializeMinimapCamera(player);

        interationManager = new InteractionManager(player, uiManager, followCamera);

        SoundManager.sInst.PlayBGM(bgm);
    }
}
