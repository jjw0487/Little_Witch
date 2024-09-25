using Cysharp.Threading.Tasks;
using Enums;
using System;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshPlayer : Player
{
    [SerializeField] private PlayerStreetMovementDataConfig data;
    [SerializeField] private NavMeshAgent nav;

    [SerializeField] private Transform skillRange;
    [SerializeField] private Transform attackPoint;

    [SerializeField] private PlayerDestinationPoint destinationPoint;

    private PlayerController controller;
    private NavMeshPlayerStateController state;
    private SkillReferenceData currentSkill;

    private bool isStunned = false;

    private bool isMoving;
    private bool isRunning;

    private bool isCasting;
    private bool isCancelled;
    private bool isRayInRange;

    private Vector3 hitPoint;
    public override bool IsDead() => controller.IsDead();
    public override bool IsAbleToUseSkill() => !isStunned && !isCasting;
    public bool IsTargetAttackable(RaycastHit hit)
        => Vector3.Distance(hit.point, Position()) < 5f;
    public override Vector3 Position()
        => this.transform.position;
    protected override void DecreaseStaminaValue(float value)
        => controller.DecreaseStaminaValue(value);
    public override bool IsSwitchable()
        => state.GetStateType() == StateType.Idle;

    public override void PlayerUpdate()
    {
        state.StateUpdate();

        if (Input.GetKeyDown(KeyCode.S)) StopPlayer(StateType.Idle);
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
    public override void StartConversation(Transform target)
    {
        StopPlayer(StateType.Idle);
        UpdateRotation(target.position);
    }

    public void UpdateRotation(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        dir.y = 0; // Y축만 회전하기 위해 dir 벡터의 X와 Z값만 유지하고, Y값은 0으로 설정
        if (dir != Vector3.zero)
        {// LookRotation을 사용해 Y축 회전만 적용
            transform.rotation = Quaternion.LookRotation(dir.normalized);
        }


        /*// 출처 : https://srdeveloper.tistory.com/115

        //※ 3차원 공간에서는 전진 방향이 z축이니 Vector2의 x값을 transform.position.z값으로 설정한다.
        //※ steeringTarget: 경로상의 다음 목적지.
        Vector2 forward = new Vector2(transform.position.z, transform.position.x);
        Vector2 steeringTarget = new Vector2(nav.steeringTarget.z, nav.steeringTarget.x);

        //방향을 구한 뒤, 역함수로 각을 구한다.
        Vector2 dir = steeringTarget - forward;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        //방향 적용
        transform.eulerAngles = Vector3.up * angle;*/
    }



    private void UpdateNavMeshRotation()
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
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    PlayerRunning(true);
                    DecreaseStaminaValue(0.1f); // 스테미너 소모
                }
                else PlayerRunning(false);

            }
            else PlayerRunning(false);
        }

    }
    private void PlayerRunning(bool value)
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
    public void StopPlayer(StateType type)
    {
        nav.SetDestination(transform.position);

        isMoving = false;
        isRunning = false;
        anim.SetBool("IsRunning", false);
        anim.SetBool("IsWalking", false);
        state.ChangeState(type);
    }
    public void MoveRayPoint(RaycastHit hit)
    {
        SetDestination(hit.point);

        if (!isMoving)
        {
            isMoving = true;
            nav.speed = data.speed;
            anim.SetBool("IsWalking", true);
        }
    }
    public void StepBeforeNormalAttack(IMonster target)
    {
        //nav.SetDestination(target.Position());

        // 타겟까지 걸어간 후 성공하면 콜백으로 일반공격을 해야 하는데...

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

    public void CancelUsingSkill()
    {
        isCancelled = true;
    }

    public void NormalAttack(Vector3 hit)
    {
        hitPoint = hit;
        EventManager.normalAttackEvent();
    }
    
    public override void UseSkill(SkillReferenceData skill,
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
        else // 스킬 시전 위치를 기다리는 스킬
        {
            SkillRange(skill.SkillRange, 5f, slotCallback, controllerCallback);
        }
    }

    private async void SkillRange(float range, float duration,
        Action slotCallback, Action controllerCallback)
    {
        anim.SetTrigger("SkillRange");

        skillRange.localScale = new Vector3(range, 0.001f, range);
        skillRange.gameObject.SetActive(true);

        if (await WaitForFeedback(duration))
        {
            skillRange.gameObject.SetActive(false);
            CastingAnimation(currentSkill.CastingDuration, currentSkill.Trigger).Forget();
            slotCallback();
            controllerCallback();
        }
        else
        {
            skillRange.gameObject.SetActive(false);
            state.ChangeState(StateType.Idle);
            anim.SetTrigger("Idle");
            isCasting = false;
        }
    }

    private async UniTaskVoid CastingAnimation(float duration, string trigger)
    {
        UpdateRotation(hitPoint);

        anim.SetTrigger(trigger);

        await UniTask.WhenAny(
            UniTask.WaitUntil(() => isStunned), // 얻어 맞아서 취소되거나
            UniTask.Delay(TimeSpan.FromSeconds(duration), // 캐스팅 시간이 전부 소모되었을 때
            cancellationToken: source.Token));

        if (!isStunned) state.ChangeState(StateType.Idle);

        isCasting = false;
    }


    private async UniTask<bool> WaitForFeedback(float duration)
    {
        isRayInRange = false;

        return await UniTask.WhenAny(
            UniTask.WaitUntil(() => isRayInRange),
            UniTask.WaitUntil(() => isCancelled),
            UniTask.WaitUntil(() => isStunned),
            UniTask.Delay(TimeSpan.FromSeconds(duration),
            cancellationToken: source.Token)) == 0;
    }

    

    public void SkillEffect()
    {
        currentSkill.SkillEffect(attackPoint.position,
            hitPoint, this.transform, controller.GetSkillPower());
    }



    #endregion Attack

    public override void GetHit(float dmg)
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

    public override void Die()
    {
        source.Cancel();
        source.Dispose();

        // 죽는 모션 애니메이션을 만들자 !!
        SceneLoader.sInst.LoadScene(0);

        state.ChangeState(StateType.Die);
    }

    private void SetDestination(Vector3 pos)
    {
        //Debug.Log($"hit point [x : {pos.x}] [y : {pos.y}] [z : {pos.z}]");

        nav.SetDestination(pos);
        destinationPoint.SpotPoint(pos);
    }

    public override void FindInteractableTarget()
    {
        Collider[] hitColliders = 
            Physics.OverlapSphere(this.transform.position, 0.1f);

        foreach (Collider col in hitColliders)
        {
            //Debug.Log("col name : " + col.name);

            if (!col.isTrigger) continue;

            if (col.TryGetComponent(out Npc value))
            {
                value.StartConversation(this.transform);
                return;
            }
        }
    }

}
