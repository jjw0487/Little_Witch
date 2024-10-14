using System;
using System.Threading;
using UnityEngine;

/// <summary>
/// Nav Mesh Agent와 Rigidbody 플레이어에서 공통적으로 사용될 코드 추상화
/// </summary>
public abstract class Player : MonoBehaviour, IPlayer
{
    [SerializeField] protected Animator anim;
    [SerializeField] protected Transform staminaGaugePoint;

    protected CancellationTokenSource source;
    private void OnEnable() => source = new CancellationTokenSource();
    private void OnDisable()
    {
        source.Cancel();
        source.Dispose();
    }
    public abstract Vector3 Position(); 
    public abstract bool IsDead(); // 플레이어가 사망한 상태인지
    public abstract bool IsSwitchable(); // 플레이어 타입이 전환 가능한 상태인지
    public abstract bool IsAbleToUseSkill(); // 스킬이 사용 가능한지

    public abstract void InitializeMovement(PlayerController _controller);

    public abstract void Spawn(Transform spawnPosition); // 전환된 플레이어 Active
    public abstract void Despawn(); // 전환된 플레이어 Inactive
    
    public abstract void PlayerUpdate(); // 전환된 플레이어의 Update()
    public abstract void PlayerFixedUpdate(); // 전환된 플레이어의 FixedUpdate()

    protected abstract void DecreaseStaminaValue(float value); // 스태미너 수치 감소

    public abstract void GetHit(float dmg); // 공격 당함
    public abstract void Die(); // 사망

    public abstract void UseSkill(SkillReferenceData skill, 
        Action slotCallback, Action controllerCallback); // 스킬 사용

    public abstract void FindInteractableTarget(); // 상호작용 가능한 대상 찾기

    public abstract void StartConversation(Transform transform); // 전환된 플레이어 위치에서 대화 실행
    
    public virtual Transform StaminaGaugePosition() => staminaGaugePoint; // 노출될 스태미너 UI 위치

    public virtual void PickUpItem() // 아이템 획득 Keycode 이벤트
    {
        Collider[] hitColliders =
            Physics.OverlapSphere(this.transform.position, 1.5f);

        foreach (Collider col in hitColliders)
        {
            if (col.isTrigger) continue;

            if (col.TryGetComponent(out ItemObject value))
            {
                value.ItemPickedUp();
            }
        }
    }

}
