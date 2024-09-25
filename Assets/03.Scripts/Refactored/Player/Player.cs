using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Enums;
using UnityEngine;

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
    public abstract bool IsDead();
    public abstract bool IsSwitchable();
    public abstract bool IsAbleToUseSkill();
    public abstract void InitializeMovement(PlayerController _controller);
    public abstract void Spawn(Transform spawnPosition);
    public abstract void Despawn();
    
    public abstract void PlayerUpdate();
    public abstract void PlayerFixedUpdate();
    protected abstract void DecreaseStaminaValue(float value);
    public abstract void GetHit(float dmg);
    public abstract void Die();
    public abstract void UseSkill(SkillReferenceData skill, 
        Action slotCallback, Action controllerCallback);
    public abstract void FindInteractableTarget();
    public abstract void StartConversation(Transform transform);
    public abstract Vector3 Position();
    public virtual Transform StaminaGaugePosition() => staminaGaugePoint;
    public virtual void PickUpItem()
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
