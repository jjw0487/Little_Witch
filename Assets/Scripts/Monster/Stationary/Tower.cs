using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

/// <summary>
/// 플레이어의 이동을 방해하는 장애물, 플레이어에게 화살을 발사함
/// 공격 받지 않음
/// </summary>
public class Tower : MonoBehaviour
{
    [SerializeField] private float attackSpeed = 3f; // 공격 속도

    [SerializeField] private Transform tr_Neck; // 플레이어의 방향대로 회전할 지지대
    [SerializeField] private Transform tr_AttackPoint;

    [SerializeField] private Arrow arrow; // 화살

    [SerializeField] private AudioClip arrowSound;

    private CancellationTokenSource source;
    private IPlayer target;
    private bool inBattle;

    private void OnDisable()
    {
        source.Cancel();
        source.Dispose();
    }
    private void Start()
    {
        source = new();
    }
    private async UniTask<bool> WaitForFeedback(float duration)
    {
        return await UniTask.WhenAny(
            UniTask.WaitUntil(() => !inBattle),
            UniTask.Delay(TimeSpan.FromSeconds(duration),
            cancellationToken: source.Token)) == 1;
    }
    private void FixedUpdate()
    {
        if(inBattle) tr_Neck.rotation = LookAtTarget();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IPlayer value))
        {
            target = value;

            if(!inBattle)
            {
                inBattle = true;
                BattleMode().Forget();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IPlayer value))
        {
            inBattle = false;
        }
    }

    private Quaternion LookAtTarget()
    {
        return Quaternion.LookRotation((target.Position() - tr_AttackPoint.position).normalized);
    }

    protected virtual async UniTaskVoid BattleMode() // 플레이어가 범위 내에 들어왔을 때 범위 밖으로 이동 전까지 계속 공격을 실행
    {
        await UniTask.Delay(System.TimeSpan.FromSeconds(1f),
            cancellationToken: source.Token);

        if (!inBattle) return;

        NextAttackDelay().Forget();

        async UniTaskVoid NextAttackDelay()
        {
            if (target.IsDead())
            {
                inBattle = false; return;
            }

            SoundManager.sInst.Play(arrowSound);
            arrow.OnFire(tr_AttackPoint.position, target.Position());

            bool nextAttack = await WaitForFeedback(attackSpeed);

            if (nextAttack)
            {
                NextAttackDelay().Forget();
            }
        }
    }
}
