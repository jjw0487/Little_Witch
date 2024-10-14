using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

/// <summary>
/// 발동되면 전방으로 레이저를 노출하여 검출되는 플레이어에 데미지를 준다.
/// </summary>
public class SecurityBot : MonoBehaviour
{
    [SerializeField] private AudioClip laserAudio;

    [SerializeField] private float attackSpeed = 7f; // 공격 재실행 delay

    [SerializeField] private Animator anim;
    [SerializeField] private Laser laser; 

    private CancellationTokenSource source;
    private IPlayer target;
    private bool inBattle;

    private bool machineStopped = false; // 클리어되어 작동을 멈춰야 할 때

    private void OnDisable()
    {
        source.Cancel();
        source.Dispose();
    }
    private void Start()
    {
        source = new();
    }
    public void StopMachine()
    {
        machineStopped = true; // 클리어되어 작동을 멈춰야 할 때
    }
    public void LaserSound()
    {
        SoundManager.sInst.Play(laserAudio);
    }

    private void OnTriggerEnter(Collider other) // 플레이어가 범위 내로 진입하였을 때
    {
        if (machineStopped) return;

        if (other.gameObject.TryGetComponent(out IPlayer value))
        {
            target = value;

            if (!inBattle)
            {
                inBattle = true;
                BattleMode().Forget();
            }
        }
    }

    private void OnTriggerExit(Collider other) // 플레이어가 범위 밖으로 나갔을 때
    {
        if (machineStopped) return;

        if (other.gameObject.TryGetComponent(out IPlayer value))
        {
            inBattle = false;
        }
    }

    protected virtual async UniTaskVoid BattleMode()
    {
        await UniTask.Delay(System.TimeSpan.FromSeconds(3f),
            cancellationToken: source.Token);

        if (!inBattle) return;

        if (machineStopped) return;

        NextAttackDelay().Forget();

        async UniTaskVoid NextAttackDelay()
        {
            if (machineStopped) return;

            if (target.IsDead())
            {
                inBattle = false; return;
            }

            anim.SetTrigger("Attack");

            bool nextAttack = await WaitForFeedback(attackSpeed);

            if (nextAttack)
            {
                NextAttackDelay().Forget();
            }
        }
    }

    private async UniTask<bool> WaitForFeedback(float duration)
    {
        return await UniTask.WhenAny(
            UniTask.WaitUntil(() => !inBattle),
            UniTask.Delay(TimeSpan.FromSeconds(duration),
            cancellationToken: source.Token)) == 1;
    }
}
