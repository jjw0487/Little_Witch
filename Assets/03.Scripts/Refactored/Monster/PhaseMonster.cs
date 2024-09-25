using Cysharp.Threading.Tasks;
using Enums;
using System.Threading;
using UnityEngine;

/// <summary>
///  스포너로 생성되는 몬스터와 다르게 고유의 위치를 가지고 있다 
/// </summary>

public class PhaseMonster : Monster
{


    [SerializeField] private bool isAutoSpawn = false;

    protected Vector3 orgPosition;

    protected virtual void Start()
    {
        orgPosition = this.transform.position;

        if (isAutoSpawn)
        {
            Initialize(null);
            Spawn(orgPosition);
        }
    }


    public override void Spawn(Vector3 spawnPos)
    {
        source = new CancellationTokenSource();

        //this.transform.position = orgPosition; // <- Changed

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


    public override void Roam()
    {
        if(hp > 0)
        {
            nav.isStopped = false;
            nav.SetDestination(orgPosition);
            isMoving = true;
            anim.SetBool("IsRunning", true);
        }
    }

    public override void Roaming()
    {
        if (isMoving)
        {
            if (nav.pathPending) return;

            if (nav.remainingDistance < 0.3f) // 0.3f <- Changed
            {
                isMoving = false;

                anim.SetBool("IsRunning", false);
            }
        }
    }
    public override void FollowTarget()
    {
        if (!nav.pathPending)
        {
            if (!targetOn) return; // <- Changed

            if (nav.destination != target.Position())
            {
                nav.destination = target.Position();
            }

            if (IsAbleToAttack())
            {
                state.ChangeState(StateType.Attack);
            }
        }
    }
    public override void Attack()
    {
        inBattle = true;
        nav.isStopped = true;
        anim.SetBool("IsRunning", false);
        BattleMode().Forget();
    }

    protected override async UniTaskVoid BattleMode()
    {
        this.transform.rotation = LookAtTarget();

        await UniTask.Delay(System.TimeSpan.FromSeconds(0.3f),
            cancellationToken: source.Token);

        if (isStunned) return;

        NextAttackDelay().Forget();

        async UniTaskVoid NextAttackDelay()
        {
            if (!targetOn) return; // <- Changed

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

            if (!targetOn) return; // <- Changed

            if (isStunned) return;

            if (IsAbleToAttack()) NextAttackDelay().Forget();
            else state.ChangeState(StateType.Move);
        }
    }


    protected override void OnTriggerEnter(Collider other) { }

    public virtual void TargetOnManually(IPlayer value)
    {
        if (!isAlive) return;

        if (hp <= 0) return;

        if (target != null) return;

        TargetOn(value);
    }

    public virtual void LostTargetManually()
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
}
