using Cysharp.Threading.Tasks;
using Enums;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour, IMonster
{
    //mutual data
    [Header("Monster Data")]
    [SerializeField] protected MonsterData data;

    [SerializeField] protected AudioClip getHit;
    [SerializeField] private AudioClip attack;

    [SerializeField] protected Renderer bodyRenderer;
    [SerializeField] protected Animator anim;
    [SerializeField] protected NavMeshAgent nav;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected Transform hpPosition;
    [SerializeField] protected Transform dmgPosition;
    [SerializeField] protected float hitPointYOffset;

    protected MonsterStateController state;
    protected MonsterHPSlider hpSlider;
    protected Vector3 targetDir;
    protected IPlayer target;
    protected float hp;

    protected float hitDelayTimer = 0f;

    protected bool targetOn = false;
    protected bool isMoving;
    protected bool isAlive;
    protected bool inBattle;
    protected bool isStunned; // 맞아서 스턴 된 상태

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
    public virtual void Initialize(Action _deathCountCallback)
    {
        deathCountCallback = _deathCountCallback;

        state = new MonsterStateController(this);
    }

    public virtual void Spawn(Vector3 spawnPos)
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
    public virtual void Roam()
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


    public virtual void Roaming()
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
    public virtual bool IsAbleToAttack()
    {
        if (!targetOn) return false;

        targetDir = target.Position() - this.transform.position;

        return targetDir.magnitude < data.STKDist;
    }

    public virtual void Attack()
    {
        inBattle = true;
        nav.isStopped = true;
        anim.SetBool("IsRunning", false);
        BattleMode().Forget();
    }

    protected virtual async UniTaskVoid BattleMode()
    {
        this.transform.rotation = LookAtTarget();

        await UniTask.Delay(System.TimeSpan.FromSeconds(0.3f),
            cancellationToken: source.Token);

        if (isStunned) return;

        NextAttackDelay().Forget();

        async UniTaskVoid NextAttackDelay()
        {
            if (target.IsDead())
            {
                LostTarget(); return;
            }

            if (IsAbleToAttack())
            {
                this.transform.rotation = LookAtTarget();
                anim.SetTrigger("Attack");
            }
            else
            {
                state.ChangeState(StateType.Move);
                return;
            }

            await UniTask.Delay(System.TimeSpan.FromSeconds(data.AttackSpeed),
            cancellationToken: source.Token);

            if (isStunned) return;

            if (IsAbleToAttack()) NextAttackDelay().Forget();
            else state.ChangeState(StateType.Move);
        }
    }

    
    #endregion Attack

    #region GetHit


    public virtual void GetHit(float dmg)
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

        async UniTaskVoid HitDelay(float duration)
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

    protected virtual void HuntingQuestEvent() { } // 퀘스트 타겟 몬스터일 경우 본인 .cs 에서 이벤트 추가


    
    protected virtual async UniTaskVoid OnDebuff(float duration, float percentage)
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

    protected virtual async UniTaskVoid DelayedChangeState(StateType newType, float duration)
    {
        inBattle = false;

        state.ChangeState(StateType.Pending);

        await UniTask.Delay(System.TimeSpan.FromSeconds(duration), cancellationToken: source.Token);

        state.ChangeState(newType);
    }
    public virtual void ChangeTarget(IPlayer _target)
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

    protected virtual void OnTriggerEnter(Collider other)
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
