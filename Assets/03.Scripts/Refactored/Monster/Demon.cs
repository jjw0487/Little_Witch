using Cysharp.Threading.Tasks;
using Enums;
using UnityEngine;

public class Demon : PhaseMonster
{
    [SerializeField] private AudioClip[] attack;
    [SerializeField] private AudioClip death;
    [SerializeField] private DemonProjectileAttack projectile;
    [SerializeField] private Transform projectileAttackPoint;

    private int attackType = 0;
    private float attackDist = 0f; // 공격 타입마다 변하는 공격 가능 거리
    private float attackPower = 0f; // 공격 타입마다 변하는 공격력

    protected override void Start()
    {
        base.Start();

        ChangeNextRandomAttack();
    }

    private void RandomAttack()
    {
        anim.SetTrigger($"Attack_{attackType}");
    }

    private void ChangeNextRandomAttack()
    {
        attackType = new System.Random().Next(0, 3);

        attackDist = attackType == 2 ? 4f : 2f;
        attackPower = attackType == 2 ? 65f : 80f;
    }

    public void SlashAttack() { SoundManager.sInst.Play(attack[1]); }

    public void BiteAttack() { SoundManager.sInst.Play(attack[0], 0.7f); }

    public void ProjectileAttack()
    {
        // 멀리서 구체를 발사하는 공격

        var obj = Instantiate(projectile, projectileAttackPoint.position, projectileAttackPoint.rotation);

        obj.Initialize(target.Position(), attackPower);

        Debug.Log("Boss Projectile Attack");
    }

    public override bool IsAbleToAttack()
    {
        if (!targetOn) return false;
        targetDir = target.Position() - this.transform.position;
        return targetDir.magnitude < attackDist; // <- Changed
    }

    #region Attack

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
                RandomAttack();
            }
            else
            {
                ChangeNextRandomAttack();
                state.ChangeState(StateType.Move);
                return;
            }

            await UniTask.Delay(System.TimeSpan.FromSeconds(data.AttackSpeed),
            cancellationToken: source.Token);

            if (!targetOn) return; // <- Changed

            if (isStunned) return;

            if (IsAbleToAttack()) NextAttackDelay().Forget();
            else 
            {
                ChangeNextRandomAttack();
                state.ChangeState(StateType.Move);
            }
            
        }
    }

    #endregion Attack

    #region GetHit
    public override void GetHit(float dmg)
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
            SoundManager.sInst.Play(death);

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

    #endregion GetHit

    #region AnimationEvent
    public override void AttackOverlapSphere()
    {
        Collider[] hitColliders = Physics.OverlapSphere(attackPoint.position,
            data.AttackRadius);
        foreach (Collider col in hitColliders)
        {
            if (!col.isTrigger)
            {
                if (col.TryGetComponent(out IPlayer value))
                {
                    value.GetHit(attackPower); // <- Changed
                    return;
                }
            }
        }
    }

    #endregion AnimationEvent

}
