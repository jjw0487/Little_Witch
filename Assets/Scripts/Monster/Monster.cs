using Cysharp.Threading.Tasks;
using Enums;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// NavMeshAgent를 통해 움직이는 필드 몬스터
/// </summary>
public class Monster : MonoBehaviour, IMonster
{
    // mutual data
    [Header("Monster Data")]
    [SerializeField] protected MonsterData data;

    // 몬스터마다 공격, 피격 사운드가 달라 사운드를 직접 갖고 필요시에 실행한다
    [SerializeField] protected AudioClip getHit;
    [SerializeField] private AudioClip attack;

    [SerializeField] protected Renderer bodyRenderer; // 마우스 호버 되었을 때 메터리얼 색상 변경
    [SerializeField] protected Animator anim; // 애니메이터
    [SerializeField] protected NavMeshAgent nav; 

    [SerializeField] protected Transform attackPoint; // 공격 위치
    [SerializeField] protected Transform hpPosition; // hp slider 위치
    [SerializeField] protected Transform dmgPosition; // floating damage 위치
    [SerializeField] protected float hitPointYOffset; // 타겟팅 스킬이 향할 위치

    protected MonsterStateController state; // FSM
    protected MonsterHPSlider hpSlider; // hp slider 캐싱

    protected Vector3 targetDir;
    protected IPlayer target;
    protected float hp;

    protected float hitDelayTimer = 0f;

    protected bool targetOn = false;
    protected bool isMoving;
    protected bool isAlive;
    protected bool inBattle;
    protected bool isStunned; // 맞아서 잠시 스턴 된 상태, 실행중인 행동 취소

    protected CancellationTokenSource source;
    protected Action deathCountCallback;
    public bool IsAlive() => isAlive;
    public Vector3 Position() => this.transform.position;
    public Vector3 HitPoint() => new Vector3(this.transform.position.x,
        this.transform.position.y + hitPointYOffset, this.transform.position.z);
    protected virtual Quaternion LookAtTarget() =>
        Quaternion.LookRotation((target.Position() - transform.position).normalized);
    public virtual void Debuff(float duration, float percentage)
        => OnDebuff(duration, percentage).Forget();
    protected virtual void HuntingQuestEvent() { } // 퀘스트 타겟 몬스터일 경우 본인 .cs 에서 이벤트 추가
    private void Update()
    {
        if(!isStunned)
        {
            state.StateUpdate();
        }
    }

    private void FixedUpdate()
    {
        if (!isStunned)
        {
            state.FixedStateUpdate();
        }

        if (targetOn)
        {
            hpSlider.FollowTarget(hpPosition.transform.position);
        }
    }

    
    #region Spawn
    public virtual void Initialize(Action _deathCountCallback) // Pool에서 생성
    {
        deathCountCallback = _deathCountCallback;

        state = new MonsterStateController(this);
    }

    public virtual void Spawn(Vector3 spawnPos) // 소환
    {
        source = new CancellationTokenSource();

        this.transform.position = spawnPos;

        nav.enabled = true;

        isAlive = true;
        inBattle = false;
        isStunned = false;
        hp = data.HP;
        nav.speed = data.Speed;
        nav.stoppingDistance = data.StopDist;
        bodyRenderer.material.color = Color.white;

        this.gameObject.SetActive(true);

        state.InitializeState(StateType.Idle);
    }

    public void Despawn()
    {
        source.Cancel();
        source.Dispose();

        target = null;
        targetOn = false;
        hpSlider = null;
        isAlive = false;

        this.gameObject.SetActive(false);
    }
    
    #endregion Spawn

    #region Idle
    public virtual void Roam() // Roaming State 진입 시 
    {
        inBattle = false;

        nav.isStopped = false;

        Vector3 randPos = new Vector3();

        randPos.Set(transform.position.x + UnityEngine.Random.Range(-2.5f, 2.5f), 0f,
            transform.position.z + UnityEngine.Random.Range(-2.5f, 2.5f));

        nav.SetDestination(randPos);

        isMoving = true;

        anim.SetBool("IsRunning", true);
    }


    public virtual void Roaming() // 로밍 ( 로밍 몬스터 )
    {
        if (isMoving)
        {
            if (nav.pathPending) return;

            if (nav.remainingDistance < 0.2f)
            {
                isMoving = false;

                anim.SetBool("IsRunning", false);
            }
        }
    }
    #endregion Idle

    #region Move
    public virtual void Move()
    {
        nav.isStopped = false;

        nav.SetDestination(target.Position());

        isMoving = true;

        anim.SetBool("IsRunning", true);
    }

    public virtual void FollowTarget()
    {
        if (!nav.pathPending)
        {
            if (nav.destination != target.Position())
            {
                nav.destination = target.Position();
            }

            if (IsAbleToAttack())
            {
                state.ChangeState(StateType.Attack);
            }

            if (Vector3.Distance(this.transform.position, target.Position()) > 10f)
            {
                LostTarget();
            }
        }
    }
    #endregion Move

    #region Attack
    public virtual bool IsAbleToAttack() // 공격이 가능한 범위인지 확인
    {
        if (!targetOn) return false;

        targetDir = target.Position() - this.transform.position;

        return targetDir.magnitude < data.STKDist;
    }

    public virtual void Attack() // Attack State 진입 시
    {
        inBattle = true;
        nav.isStopped = true;
        anim.SetBool("IsRunning", false);
        BattleMode().Forget();
    }

    protected virtual async UniTaskVoid BattleMode() // 몬스터 공격
    {
        this.transform.rotation = LookAtTarget(); // 즉시 타겟을 바라봄

        await UniTask.Delay(System.TimeSpan.FromSeconds(0.3f), // 잠시 딜레이
            cancellationToken: source.Token);

        if (isStunned) return;

        NextAttackDelay().Forget();

        async UniTaskVoid NextAttackDelay()
        {
            if (target.IsDead()) // 타겟이 죽었다면 실행 취소
            {
                LostTarget(); return;
            }

            if (IsAbleToAttack()) // 공격 가능한 위치라면
            {
                this.transform.rotation = LookAtTarget();
                anim.SetTrigger("Attack"); // 공격
            }
            else // 아니라면
            {
                state.ChangeState(StateType.Move); // 다시 타겟을 쫓음
                return;
            }

            await UniTask.Delay(System.TimeSpan.FromSeconds(data.AttackSpeed), // 재 공격 전 딜레이
            cancellationToken: source.Token);

            if (isStunned) return;

            if (IsAbleToAttack()) NextAttackDelay().Forget();
            else state.ChangeState(StateType.Move);
        }
    }
    #endregion Attack

    #region GetHit


    public virtual void GetHit(float dmg) // 피격당할 시 즉시 하던 행동을 멈추고 그 자리에서 애니메이션 실행
    {
        if (hp <= 0) return; // 죽은 후 딜레이동안 들어오는 공격 무효

        isStunned = true;

        nav.SetDestination(transform.position);
        float damage = UnityEngine.Random.Range(dmg * 0.8f, dmg * 1.2f) - data.DP;
        hp -= damage < 1 ? 0 : damage;

        EventManager.floatingDamageEvent(dmgPosition.position, damage);

        hpSlider?.GetHit(hp);

        if (hp > 0)
        {
            anim.SetTrigger("IsHit");
            SoundManager.sInst.Play(getHit);
            if (hitDelayTimer <= 0f) HitDelay(0.7f).Forget();
            else hitDelayTimer = 0.7f;
        }
        else
        {
            anim.SetTrigger("Death");
            nav.enabled = false;
            hpSlider?.Despawn();
            
            HuntingQuestEvent();
            data.GoldAndExpEvent();
            
            DelayedChangeState(StateType.Die, 3f).Forget();
        }

        async UniTaskVoid HitDelay(float duration) // 기절 ( 기절 할 시간 )
        {
            hitDelayTimer += duration;

            while (hitDelayTimer > 0f)
            {
                hitDelayTimer -= Time.deltaTime;
                await UniTask.Yield();  // 매 프레임마다 타이머를 확인
            }

            isStunned = false;

            if (inBattle)
            {
                if (IsAbleToAttack()) BattleMode().Forget();
                else state.ChangeState(StateType.Move);
            }
        }
    }
    
    protected virtual async UniTaskVoid OnDebuff(float duration, float percentage) // 디버프 공격 받음
    {
        bodyRenderer.material.color = Color.cyan;
        nav.speed = data.Speed * percentage;

        await UniTask.Delay(System.TimeSpan.FromSeconds(duration), cancellationToken: source.Token);

        bodyRenderer.material.color = Color.white;
        nav.speed = data.Speed;
    }

    #endregion GetHit

    #region Die
    public virtual void Die()
    {
        Vector3 dropPosition = new Vector3(Position().x, Position().y + 2f, Position().z);

        data.DropItem(dropPosition);

        if(deathCountCallback != null) deathCountCallback.Invoke(); // 몬스터 스포너에 죽었다는 사실을 알림

        Despawn();
    }
    #endregion Die

    #region AnimationEvent
    public virtual void AttackOverlapSphere()
    {
        if(attack != null)
        {
            SoundManager.sInst.Play(attack);
        }

        Collider[] hitColliders = Physics.OverlapSphere(attackPoint.position,
            data.AttackRadius);
        foreach (Collider col in hitColliders)
        {
            if (!col.isTrigger)
            {
                if (col.TryGetComponent(out IPlayer value))
                {
                    value.GetHit(data.ATP);
                    return;
                }
            }
        }
    }

    #endregion AnimationEvent

    protected virtual async UniTaskVoid DelayedChangeState(StateType newType, float duration) // 스테이트 변경
    {
        inBattle = false;

        state.ChangeState(StateType.Pending);

        await UniTask.Delay(System.TimeSpan.FromSeconds(duration), cancellationToken: source.Token);

        state.ChangeState(newType);
    }
    public virtual void ChangeTarget(IPlayer _target) // 타겟 변경
    {
        if(target != null) TargetOn(_target);
        else LostTarget();
    }
    public virtual void TargetOn(IPlayer newTarget)
    {
        if (!isAlive) return;
        if (hp <= 0) return;

        targetOn = true;
        target = newTarget;

        state.ChangeState(StateType.Move);

        if(hpSlider == null)
        {
            hpSlider = EventManager.monsterHPEvent();
            hpSlider.Spawn(hp, data.HP, data.HPScale);
            hpSlider.FollowTarget(hpPosition.transform.position);
        }
    }
    public virtual void LostTarget()
    {
        if (!isAlive) return;

        inBattle = false;
        targetOn = false;
        target = null;

        if (hpSlider != null)
        {
            hpSlider.Despawn();
            hpSlider = null;
        }

        state.ChangeState(StateType.Idle);
    }

    protected virtual void OnTriggerEnter(Collider other) // 타겟이 범위 내에 들어옴
    {
        if (!isAlive) return;

        if (hp <= 0) return;

        if (target != null) return;

        if (other.TryGetComponent(out IPlayer value))
        {
            TargetOn(value);
        }
    }
}
