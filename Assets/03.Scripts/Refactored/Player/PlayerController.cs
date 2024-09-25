using Enums;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayModeType previousMode;
    PlayModeType playMode = PlayModeType.None;

    [SerializeField] private BroomPlayer broom;
    [SerializeField] private NavMeshPlayer navMesh;
    [SerializeField] private GameObject dust;
 
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
        if (playMode == PlayModeType.Interact) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            player.FindInteractableTarget();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            ChangePlayeMode();
        }

        player.PlayerUpdate();
    }

    private void FixedUpdate()
    {
        if (playMode == PlayModeType.Interact) return;

        player.PlayerFixedUpdate();
    }

    
    public void InitializePlayer(Transform spawnPosition, CameraMovement _cam, PlayerStatusData _statData)
    {
        statData = _statData;
        cam = _cam;
        isDead = false;
        navMesh.InitializeMovement(this);
        broom.InitializeMovement(this);
        ChangePlayMode(PlayModeType.Street, spawnPosition);
    }

    public void FindInteractableTarget()
    {
        player.FindInteractableTarget();
    }

    public void StartConversation(Transform target)
    {
        previousMode = playMode;

        playMode = PlayModeType.Interact;

        player.StartConversation(target);

        cam.ChangePlayMode(playMode);

        cam.CameraInteractionAnimationOn();
    }

    public void EndConversation()
    {
        playMode = previousMode;

        cam.ChangePlayMode(previousMode);

        cam.CameraInteractionAnimationOff();
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

    public void ChangePlayMode(PlayModeType on, Transform spawnPosition) // broom button
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

    public bool DecreaseHP(float dmg)
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

    public bool UseSkill(SkillReferenceData skill, Action slotCallback)
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

    public void PickUpItem()
    {
        player.PickUpItem();
    }
}
