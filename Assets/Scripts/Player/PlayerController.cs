using Enums;
using System;
using UnityEngine;

/// <summary>
/// Nav Mesh Agent와 Rigidbody 으로 움직임을 구현하는 두 플레이어의 입력 이벤트를 관장
/// </summary>
public class PlayerController : MonoBehaviour
{
    PlayModeType previousMode; // 이전 플레이어 타입을 저장
    PlayModeType playMode = PlayModeType.None; // Street(Nav Mesh Agent), Broom(Rigidbody), Interact(대화), Dead

    [SerializeField] private BroomPlayer broom; // Street(Nav Mesh Agent)
    [SerializeField] private NavMeshPlayer navMesh; // Broom(Rigidbody)

    [SerializeField] private GameObject dust; // 타입이 변경될 때 이펙트
 
    private Player player;

    private bool isDead = false;
    private PlayerStatusData statData;
    private CameraMovement cam;

    private void OnEnable() => PlayerEvent.skillEvent += UseSkill;
    private void OnDisable() => PlayerEvent.skillEvent -= UseSkill;

    public bool IsDead() => isDead;
    public Vector3 Position() => player.Position();
    public Transform Transform() => player.transform;
    public Transform StaminaGaugePosition() => player.StaminaGaugePosition();
    public void DecreaseStaminaValue(float value) => statData.Stamina = -value;
    public float GetStamina() => statData.Stamina;
    public float GetSkillPower() => statData.SP;

    private void Update()
    {
        if (playMode == PlayModeType.Interact) return; // 상호작용 모드일 시 이외의 작동 불필요함

        player.PlayerUpdate(); // 현재 플레이 모드의 Update()
    }

    private void FixedUpdate()
    {
        if (playMode == PlayModeType.Interact) return; // 상호작용 모드일 시 이외의 작동 불필요함

        player.PlayerFixedUpdate(); // 현재 플레이 모드의 FixedUpdate()
    }

    
    public void InitializePlayer(Transform spawnPosition, CameraMovement _cam, PlayerStatusData _statData)
    { 
        // PlayManager 에서 씬 시작 시 전달받은 데이터로 초기화

        statData = _statData;
        cam = _cam;
        isDead = false;
        navMesh.InitializeMovement(this);
        broom.InitializeMovement(this);
        ChangePlayMode(PlayModeType.Street, spawnPosition);
    }

    public void StartConversation(Transform target) // 대화 시작
    {
        previousMode = playMode;

        playMode = PlayModeType.Interact;

        player.StartConversation(target);

        cam.ChangePlayMode(playMode); 

        cam.CameraInteractionAnimationOn(); // 카메라 위치 조정
    }

    public void EndConversation() // 대화 종료
    {
        playMode = previousMode;

        cam.ChangePlayMode(previousMode); 

        cam.CameraInteractionAnimationOff(); // 카메라 위치 조정
    }

    public void ChangePlayeMode()
    {
        if (!player.IsSwitchable()) return;

        Vector3 pos = player.Position();

        pos.y += 0.7f;

        var obj = Instantiate(dust, pos, Quaternion.identity);

        Destroy(obj, 1.2f);

        switch (playMode)
        {
            case PlayModeType.Street:
                cam.LookAtPlayerBehind(PlayModeType.Broom);
                ChangePlayMode(PlayModeType.Broom, player.transform);
                break;
            case PlayModeType.Broom:
                cam.LookAtPlayerBehind(PlayModeType.Street);
                ChangePlayMode(PlayModeType.Street, player.transform);
                break;
            default: break;
        }
    }

    public void ChangePlayMode(PlayModeType on, Transform spawnPosition) // 플레이 타입 변경
    {
        if (playMode == on) return;

        playMode = on;

        switch(playMode)
        {
            case PlayModeType.Street:
                broom.Despawn();
                navMesh.Spawn(spawnPosition);
                player = navMesh;

                cam.ChangePlayMode(PlayModeType.Street);

                break;
            case PlayModeType.Broom:
                navMesh.Despawn();
                broom.Spawn(spawnPosition);
                player = broom;

                cam.ChangePlayMode(PlayModeType.Broom);

                break;
            case PlayModeType.Dead:
                isDead = true;
                break;
            default: break;
        }
    }

    public bool DecreaseHP(float dmg) // HP 감소 이벤트 (hp가 0이하로 떨어졌는지 먼저 확인)
    {
        if (isDead) return false;

        float finalDamage = dmg - statData.DP;

        if(finalDamage <= 0) return false;

        cam.CameraGetHit();

        statData.HP = -finalDamage;

        SoundManager.sInst.Play("PlayerGetHit");

        if (statData.HP <= 0)
        {
            player.Die();
            ChangePlayMode(PlayModeType.Dead, null);
            return false;
        }

        return true;
    }

    public bool UseSkill(SkillReferenceData skill, Action slotCallback) // 스킬 사용
    {
        if (skill.MPConsumption > statData.MP) return false;

        if(!player.IsAbleToUseSkill()) return false;

        player.UseSkill(skill, slotCallback, ()=> 
        {
            // 스킬 사용 성공 시 callback
            statData.MP = -skill.MPConsumption;
        });

        return true;
    }
}
