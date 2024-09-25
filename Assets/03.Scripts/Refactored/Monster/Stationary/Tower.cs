using Cysharp.Threading.Tasks;
using Enums;
using System;
using System.Threading;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private float attackSpeed = 3f;

    [SerializeField] private Transform tr_Neck;
    [SerializeField] private Transform tr_AttackPoint;

    [SerializeField] private Arrow arrow;

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
        //Quaternion rot = Quaternion.LookRotation(
        //(target.Position() + new Vector3(0f, 1f, 0f) - tr_AttackPoint.position).normalized);

        return Quaternion.LookRotation((target.Position() - tr_AttackPoint.position).normalized);
    }

    protected virtual async UniTaskVoid BattleMode()
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

    private async UniTask<bool> WaitForFeedback(float duration)
    {
        return await UniTask.WhenAny(
            UniTask.WaitUntil(() => !inBattle),
            UniTask.Delay(TimeSpan.FromSeconds(duration),
            cancellationToken: source.Token)) == 1;
    }


}
