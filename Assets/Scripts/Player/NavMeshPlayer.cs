using Cysharp.Threading.Tasks;
using Enums;
using System;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ���ڷ縦 Ÿ�� ���� ����, ���콺�� Ŭ���Ͽ� Raycast�� ����� ��ġ�� Nav mesh Agent�� �̿��Ͽ� �÷��̾ �̵�
/// </summary>
public class NavMeshPlayer : Player
{
    [SerializeField] private PlayerStreetMovementDataConfig data; // �̵��ӵ�, ȸ���ӵ� ��

    [SerializeField] private NavMeshAgent nav;

    [SerializeField] private Transform skillRange; // ��ų ��� ���� ����Ʈ

    [SerializeField] private PlayerDestinationPoint destinationPoint; // Nav Mesh Agent�� Destination�� ǥ������ ����Ʈ

    [SerializeField] private Transform attackPoint; // ���� ���� ����

    private PlayerController controller;

    private NavMeshPlayerStateController state; // FSM

    private SkillReferenceData currentSkill; // ĳ���� �ð� ���� ������ ������ ������� ��ų ������

    private bool isStunned = false;

    private bool isMoving;
    private bool isRunning;

    private bool isCasting;
    private bool isCancelled;
    private bool isRayInRange;

    private Vector3 hitPoint; 
    public override bool IsDead() => controller.IsDead();
    public override bool IsAbleToUseSkill() => !isStunned && !isCasting; // ��ų ����� ��������
    public bool IsTargetAttackable(RaycastHit hit) // �Ϲ� ���� ��ų ���� Ȯ��
        => Vector3.Distance(hit.point, Position()) < 5f;
    public override Vector3 Position()
        => this.transform.position;
    protected override void DecreaseStaminaValue(float value) // ���¹̳� ��ġ ���� �̺�Ʈ
        => controller.DecreaseStaminaValue(value);
    public override bool IsSwitchable() // �÷��� ��� ��ȯ ��������
        => state.GetStateType() == StateType.Idle;

    public override void PlayerUpdate()
    {
        state.StateUpdate();

        if (Input.GetKeyDown(KeyCode.S)) StopPlayer(StateType.Idle); // �̵����� �÷��̾� ��� ���� (Keycode ���� �ȵǰ� ��)
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
    public override void StartConversation(Transform target) // ��ȭ ��ȣ�ۿ�
    {
        StopPlayer(StateType.Idle);
        UpdateRotation(target.position);
    }

    public void UpdateRotation(Vector3 target) // �÷��̾� target�� �ٶ󺸵��� ��� ȸ��
    {
        Vector3 dir = target - transform.position;
        dir.y = 0; // Y�ุ ȸ���ϱ� ���� dir ������ X�� Z���� �����ϰ�, Y���� 0���� ����
        if (dir != Vector3.zero)
        {// LookRotation�� ����� Y�� ȸ���� ����
            transform.rotation = Quaternion.LookRotation(dir.normalized);
        }
    }

    private void UpdateNavMeshRotation() // �÷��̾� Nav Mesh Path�� ���� ������ ȸ��
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
                if (Input.GetKey(KeyCode.LeftShift)) // �޸���
                {
                    PlayerRunning(true);
                    DecreaseStaminaValue(0.1f); // ���׹̳� �Ҹ�
                }
                else PlayerRunning(false);

            }
            else PlayerRunning(false);
        }

    }
    private void PlayerRunning(bool value) // �޸���
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
    public void StopPlayer(StateType type) // �÷��̾� ��� ����
    {
        nav.SetDestination(transform.position);

        isMoving = false;
        isRunning = false;
        anim.SetBool("IsRunning", false);
        anim.SetBool("IsWalking", false);
        state.ChangeState(type);
    }
    public void MoveRayPoint(RaycastHit hit) // ������ Ŭ���� ��ġ�� �̵� ( Walkable Ȯ�� �� )
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

    public void NormalAttack(Vector3 hit) // �Ϲ� ���� ��ų
    { 
        hitPoint = hit;
        EventManager.normalAttackEvent();
    }
    
    public override void UseSkill(SkillReferenceData skill, // ����Ű�� �����س��� ��ų ����
        Action slotCallback, Action controllerCallback)
    {
        StopPlayer(StateType.Attack);

        isCancelled = false;
        isCasting = true;

        currentSkill = skill;

        if (!skill.IsWaitBeforeAction) // ��� ���� ��ų
        {
            CastingAnimation(skill.CastingDuration, skill.Trigger).Forget();
            slotCallback();
            controllerCallback();
        }
        else // ���� ��ų (Skill Range)
        {
            SkillRange(skill.SkillRange, 5f, slotCallback, controllerCallback);
        }
    }

    private async void SkillRange(float range, float duration,
        Action slotCallback, Action controllerCallback)
    {
        anim.SetTrigger("SkillRange");

        skillRange.localScale = new Vector3(range, 0.001f, range); // ��ų ���� ǥ��
        skillRange.gameObject.SetActive(true);

        if (await WaitForFeedback(duration)) // �÷��̾��� ���� �ൿ�� ���
        {
            // ��ų ���� ( �ð� ���� ��ų ���� �� Ŭ�� �̺�Ʈ �߻� )
            skillRange.gameObject.SetActive(false);
            CastingAnimation(currentSkill.CastingDuration, currentSkill.Trigger).Forget();
            slotCallback();
            controllerCallback();
        }
        else
        {
            // ��ų ���� ��� ( �ð� �ʰ� Ȥ�� ���� �ۿ� Ŭ�� �̺�Ʈ �߻� )
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
            UniTask.WaitUntil(() => isRayInRange), // ��ų ���� Ȯ��
            UniTask.WaitUntil(() => isCancelled), // ĳ���� ���, ��� �̺�Ʈ
            UniTask.WaitUntil(() => isStunned), // ĳ���� ���� ����
            UniTask.Delay(TimeSpan.FromSeconds(duration), // �ð� �ʰ�
            cancellationToken: source.Token)) == 0;
    }

    private async UniTaskVoid CastingAnimation(float duration, string trigger)
    {
        UpdateRotation(hitPoint); // ��ų ���� �������� ȸ��

        anim.SetTrigger(trigger); // ��ų ���� �ִϸ��̼�

        await UniTask.WhenAny(
            UniTask.WaitUntil(() => isStunned), // ��� �¾Ƽ� ��ҵǰų�
            UniTask.Delay(TimeSpan.FromSeconds(duration), // ĳ���� �ð��� ���� �Ҹ�Ǿ��� ��
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

    public override void GetHit(float dmg) // �÷��̾� ���ݴ���
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

    public override void Die() // �÷��̾� ���
    {
        source.Cancel();
        source.Dispose();

        SceneLoader.sInst.LoadScene(0);

        state.ChangeState(StateType.Die);
    }

    private void SetDestination(Vector3 pos) // Nav Mesh Agent �̵�
    {
        nav.SetDestination(pos);
        destinationPoint.SpotPoint(pos);
    }

    public override void FindInteractableTarget() // ��ȣ�ۿ� Keycode �Է� �� ���� ��ġ���� ��ȣ�ۿ��� ������ ����� ã��
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
