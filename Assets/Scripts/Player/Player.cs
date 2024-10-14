using System;
using System.Threading;
using UnityEngine;

/// <summary>
/// Nav Mesh Agent�� Rigidbody �÷��̾�� ���������� ���� �ڵ� �߻�ȭ
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
    public abstract bool IsDead(); // �÷��̾ ����� ��������
    public abstract bool IsSwitchable(); // �÷��̾� Ÿ���� ��ȯ ������ ��������
    public abstract bool IsAbleToUseSkill(); // ��ų�� ��� ��������

    public abstract void InitializeMovement(PlayerController _controller);

    public abstract void Spawn(Transform spawnPosition); // ��ȯ�� �÷��̾� Active
    public abstract void Despawn(); // ��ȯ�� �÷��̾� Inactive
    
    public abstract void PlayerUpdate(); // ��ȯ�� �÷��̾��� Update()
    public abstract void PlayerFixedUpdate(); // ��ȯ�� �÷��̾��� FixedUpdate()

    protected abstract void DecreaseStaminaValue(float value); // ���¹̳� ��ġ ����

    public abstract void GetHit(float dmg); // ���� ����
    public abstract void Die(); // ���

    public abstract void UseSkill(SkillReferenceData skill, 
        Action slotCallback, Action controllerCallback); // ��ų ���

    public abstract void FindInteractableTarget(); // ��ȣ�ۿ� ������ ��� ã��

    public abstract void StartConversation(Transform transform); // ��ȯ�� �÷��̾� ��ġ���� ��ȭ ����
    
    public virtual Transform StaminaGaugePosition() => staminaGaugePoint; // ����� ���¹̳� UI ��ġ

    public virtual void PickUpItem() // ������ ȹ�� Keycode �̺�Ʈ
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
