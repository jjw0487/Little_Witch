using Cysharp.Threading.Tasks;
using Enums;
using System;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 빗자루를 타지 않은 상태, 마우스를 클릭하여 Raycast로 검출된 위치로 Nav mesh Agent를 이용하여 플레이어를 이동
/// </summary>
public class NavMeshPlayer : Player
{
    [SerializeField] private PlayerStreetMovementDataConfig data; // 이동속도, 회전속도 등

    [SerializeField] private NavMeshAgent nav;

    [SerializeField] private Transform skillRange; // 스킬 사용 범위 이펙트

    [SerializeField] private PlayerDestinationPoint destinationPoint; // Nav Mesh Agent의 Destination을 표시해줄 이펙트

    [SerializeField] private Transform attackPoint; // 공격 시작 지점

    private PlayerController controller;

    private NavMeshPlayerStateController state; // FSM

    private SkillReferenceData currentSkill; // 캐스팅 시간 등의 이유로 시전을 대기중인 스킬 데이터

    private bool isStunned = false;

    private bool isMoving;
    private bool isRunning;

    private bool isCasting;
    private bool isCancelled;
    private bool isRayInRange;

    private Vector3 hitPoint; 
    public override bool IsDead() => controller.IsDead();
    public override bool IsAbleToUseSkill() => !isStunned && !isCasting; // 스킬 사용이 가능한지
    public bool IsTargetAttackable(RaycastHit hit) // 일반 공격 스킬 범위 확인
        => Vector3.Distance(hit.point, Position()) < 5f;
    public override Vector3 Position()
        => this.transform.position;
    protected override void DecreaseStaminaValue(float value) // 스태미너 수치 감소 이벤트
        => controller.DecreaseStaminaValue(value);
    public override bool IsSwitchable() // 플레이 모드 전환 가능한지
        => state.GetStateType() == StateType.Idle;

    public override void PlayerUpdate()
    {
        state.StateUpdate();

        if (Input.GetKeyDown(KeyCode.S)) StopPlayer(StateType.Idle); // 이동중인 플레이어 즉시 멈춤 (Keycode 변경 안되게 함)
    }

    public override void PlayerFixedUpdate()
    {
        state.FixedStateUpdate();
    }


    #region Initialize
    public override void InitializeMovement(PlayerController _controller)
    {
        controller = _controller;
        state = new NavMeshPlayerStateController(this);
        nav.enabled = false;
        nav.updateRotation = false;
    }
    public override void Spawn(Transform spawnPosition)
    {
        this.transform.position = spawnPosition.position;
        this.transform.rotation = spawnPosition.rotation;
        isStunned = false;
        nav.enabled = true;
        nav.speed = data.speed;

        this.gameObject.SetActive(true);
    }

    public override void Despawn()
    {
        nav.enabled = false;

        this.gameObject.SetActive(false);
    }
    #endregion Initialize

    #region Movement
    public override void StartConversation(Transform target) // 대화 상호작용
    {
        StopPlayer(StateType.Idle);
        UpdateRotation(target.position);
    }

    public void UpdateRotation(Vector3 target) // 플레이어 target을 바라보도록 즉시 회전
    {
        Vector3 dir = target - transform.position;
        dir.y = 0; // Y축만 회전하기 위해 dir 벡터의 X와 Z값만 유지하고, Y값은 0으로 설정
        if (dir != Vector3.zero)
        {// LookRotation을 사용해 Y축 회전만 적용
            transform.rotation = Quaternion.LookRotation(dir.normalized);
        }
    }

    private void UpdateNavMeshRotation() // 플레이어 Nav Mesh Path에 따라 서서히 회전
    {
        if (Mathf.Approximately(nav.desiredVelocity.sqrMagnitude, 0f)) return;

        Vector3 direction = nav.desiredVelocity;
        Quaternion targetAngle = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation,
            targetAngle, Time.deltaTime * data.rotSpeed);
    }
    public void Move()
    {
        if (nav.pathPending) return;

        if (isMoving)
        {
            UpdateNavMeshRotation();

            if (Input.GetKeyDown(KeyCode.S) || nav.remainingDistance < 0.1f)
            {
                StopPlayer(StateType.Idle);
                return;
            }

            if (controller.GetStamina() > 0.9f)
            {
                if (Input.GetKey(KeyCode.LeftShift)) // 달리기
                {
                    PlayerRunning(true);
                    DecreaseStaminaValue(0.1f); // 스테미너 소모
                }
                else PlayerRunning(false);

            }
            else PlayerRunning(false);
        }

    }
    private void PlayerRunning(bool value) // 달리기
    {
        if (isRunning == value) return;

        isRunning = value;

        if (value)
        {
            nav.speed = data.dashSpeed;
            anim.SetBool("IsRunning", true);
        }
        else
        {
            nav.speed = data.speed;
            anim.SetBool("IsRunning", false);
        }
    }
    public void StopPlayer(StateType type) // 플레이어 즉시 정지
    {
        nav.SetDestination(transform.position);

        isMoving = false;
        isRunning = false;
        anim.SetBool("IsRunning", false);
        anim.SetBool("IsWalking", false);
        state.ChangeState(type);
    }
    public void MoveRayPoint(RaycastHit hit) // 유저가 클릭한 위치로 이동 ( Walkable 확인 후 )
    {
        SetDestination(hit.point);

        if (!isMoving)
        {
            isMoving = true;
            nav.speed = data.speed;
            anim.SetBool("IsWalking", true);
        }
    }
    #endregion Movement

    #region Attack
    public void SkillRayPoint(RaycastHit hit)
    {
        isRayInRange = true;
        hitPoint = hit.point;
    }

    public void CancelUsingSkill() => isCancelled = true;

    public void NormalAttack(Vector3 hit) // 일반 공격 스킬
    { 
        hitPoint = hit;
        EventManager.normalAttackEvent();
    }
    
    public override void UseSkill(SkillReferenceData skill, // 단축키로 설정해놓은 스킬 시전
        Action slotCallback, Action controllerCallback)
    {
        StopPlayer(StateType.Attack);

        isCancelled = false;
        isCasting = true;

        currentSkill = skill;

        if (!skill.IsWaitBeforeAction) // 즉시 시전 스킬
        {
            CastingAnimation(skill.CastingDuration, skill.Trigger).Forget();
            slotCallback();
            controllerCallback();
        }
        else // 범위 스킬 (Skill Range)
        {
            SkillRange(skill.SkillRange, 5f, slotCallback, controllerCallback);
        }
    }

    private async void SkillRange(float range, float duration,
        Action slotCallback, Action controllerCallback)
    {
        anim.SetTrigger("SkillRange");

        skillRange.localScale = new Vector3(range, 0.001f, range); // 스킬 범위 표시
        skillRange.gameObject.SetActive(true);

        if (await WaitForFeedback(duration)) // 플레이어의 다음 행동을 대기
        {
            // 스킬 시전 ( 시간 내에 스킬 범위 내 클릭 이벤트 발생 )
            skillRange.gameObject.SetActive(false);
            CastingAnimation(currentSkill.CastingDuration, currentSkill.Trigger).Forget();
            slotCallback();
            controllerCallback();
        }
        else
        {
            // 스킬 시전 취소 ( 시간 초과 혹은 범위 밖에 클릭 이벤트 발생 )
            skillRange.gameObject.SetActive(false);
            state.ChangeState(StateType.Idle);
            anim.SetTrigger("Idle");
            isCasting = false;
        }
    }
    private async UniTask<bool> WaitForFeedback(float duration)
    {
        isRayInRange = false;

        return await UniTask.WhenAny(
            UniTask.WaitUntil(() => isRayInRange), // 스킬 범위 확인
            UniTask.WaitUntil(() => isCancelled), // 캐릭터 사망, 취소 이벤트
            UniTask.WaitUntil(() => isStunned), // 캐릭터 공격 당함
            UniTask.Delay(TimeSpan.FromSeconds(duration), // 시간 초과
            cancellationToken: source.Token)) == 0;
    }

    private async UniTaskVoid CastingAnimation(float duration, string trigger)
    {
        UpdateRotation(hitPoint); // 스킬 시전 방향으로 회전

        anim.SetTrigger(trigger); // 스킬 시전 애니메이션

        await UniTask.WhenAny(
            UniTask.WaitUntil(() => isStunned), // 얻어 맞아서 취소되거나
            UniTask.Delay(TimeSpan.FromSeconds(duration), // 캐스팅 시간이 전부 소모되었을 때
            cancellationToken: source.Token));

        if (!isStunned) state.ChangeState(StateType.Idle);

        isCasting = false;
    }

    public void SkillEffect()
    { 
        // Animation Event
        currentSkill.SkillEffect(attackPoint.position,
            hitPoint, this.transform, controller.GetSkillPower());
    }



    #endregion Attack

    public override void GetHit(float dmg) // 플레이어 공격당함
    {
        if (isStunned) return;

        isStunned = true;

        if (controller.DecreaseHP(dmg))
        {
            anim.SetTrigger("IsHit");
            StopPlayer(StateType.GetHit);
            HitDelay(0.7f).Forget();
        }
        else isStunned = false;

        async UniTaskVoid HitDelay(float duration)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: source.Token);
            state.ChangeState(StateType.Idle);
            isStunned = false;
        }
    }

    public override void Die() // 플레이어 사망
    {
        source.Cancel();
        source.Dispose();

        SceneLoader.sInst.LoadScene(0);

        state.ChangeState(StateType.Die);
    }

    private void SetDestination(Vector3 pos) // Nav Mesh Agent 이동
    {
        nav.SetDestination(pos);
        destinationPoint.SpotPoint(pos);
    }

    public override void FindInteractableTarget() // 상호작용 Keycode 입력 시 현재 위치에서 상호작용이 가능한 대상을 찾음
    {
        Collider[] hitColliders = 
            Physics.OverlapSphere(this.transform.position, 0.1f);

        foreach (Collider col in hitColliders)
        {
            if (!col.isTrigger) continue;

            if (col.TryGetComponent(out Npc value))
            {
                value.StartConversation(this.transform);
                return;
            }
        }
    }

}
