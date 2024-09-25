using Cysharp.Threading.Tasks;
using Enums;
using System;
using System.Threading;
using UnityEngine;

public class SecurityBot : MonoBehaviour
{
    [SerializeField] private AudioClip laserAudio;
    [SerializeField] private float attackSpeed = 7f;

    [SerializeField] private Animator anim;
    [SerializeField] private Laser laser;

    private CancellationTokenSource source;
    private IPlayer target;
    private bool inBattle;

    private bool machineStopped = false;
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
        machineStopped = true;
    }

    public void LaserSound()
    {
        SoundManager.sInst.Play(laserAudio);
    }

    private void OnTriggerEnter(Collider other)
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


    private void OnTriggerExit(Collider other)
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
