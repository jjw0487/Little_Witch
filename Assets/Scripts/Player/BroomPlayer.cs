using Cysharp.Threading.Tasks;
using Enums;
using System;
using UnityEngine;

/// <summary>
/// ���ڷ縦 ź ����, Rigidbody�� ���� ������
/// </summary>
public class BroomPlayer : Player
{
    [SerializeField] protected PlayerOnBroomMovementDataConfig data; // �̵��ӵ�, ȸ���ӵ� ��

    [SerializeField] private Rigidbody rb;

    [SerializeField] private Transform attackPoint; // ���� ���� ����

    [SerializeField] protected GameObject orgDashEffect; // ��� ����Ʈ

    private Vector3 dir = Vector3.zero;

    private PlayerController controller;

    private BroomPlayerStateController state; // FSM

    private Camera cam;

    private bool isStunned = false;
    private bool isGround = true;

    public override void Spawn(Transform spawnPosition)
    {
        this.transform.position = spawnPosition.position;
        this.transform.rotation = spawnPosition.rotation;
        isStunned = false;
        isGround = true;

        this.gameObject.SetActive(true);
    }
    public override void Despawn()
    {
        this.gameObject.SetActive(false);
    }
    
    public override void InitializeMovement(PlayerController _controller)
    {
        controller = _controller;

        cam = Camera.main;
        state = new BroomPlayerStateController(this);
        rb.drag = 6f;
        rb.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
    }
    public void TakeOff() // ����
    {
        if (controller.GetStamina() < 0.1f) return; // ���¹̳ʰ� ������� X

        if (this.transform.position.y < data.restricted_Flying_Height)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Vector3 jumpPower = Vector3.up * 0.3f;

                rb.AddForce(jumpPower, ForceMode.VelocityChange);

                DecreaseStaminaValue(0.1f);

                if (isGround) isGround = false;
            }
        }
    }

    public void Move() // �̵�
    {
        dir.Set(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        dir = cam.transform.rotation * dir;
        dir.y = 0f;
        dir.Normalize();

        float speed = isGround ? 2f : data.speed; // ���� ���� ����� ���� �ӵ��� �� �� ����

        rb.MovePosition(this.transform.position + dir * speed * Time.deltaTime);

        if (dir != Vector3.zero)
        {
            // ������ ���ư� �� + �������� ���ư��µ� �ݴ�������� �������� Ű�� ������ �� -�������� ȸ���ϸ鼭 ����� ������ �����ϱ����� (��ȣ�� ���� �ݴ��� ��츦 üũ�ؼ� ��¦�� �̸� �����ִ� �ڵ�) ��Ƴ׿�... 
            // ���� �ٶ󺸴� ������ ��ȣ != ���ư� ���� ��ȣ
            if (Mathf.Sign(transform.forward.x) != Mathf.Sign(dir.x) ||
                Mathf.Sign(transform.forward.z) != Mathf.Sign(dir.z))
            {
                //�츮�� �̵��� �� x �� z �ۿ� ����� ���ϹǷ�
                transform.Rotate(0, 1, 0); // ��¦�� ȸ��
                                           //�� �ݴ������ ������ ȸ�����ϴ� ���� ����
                                           //�̸� ȸ���� ���� ���Ѽ� ���ݴ��� ��츦 ����
            }

            transform.forward = Vector3.Lerp(transform.forward, dir, data.rotSpeed * Time.deltaTime);
            // ĳ������ �չ����� dir Ű���带 ���� �������� ĳ���� ȸ��

        }
    }
    public override void Die() { }

    public override void GetHit(float dmg) // �÷��̾� ���� ����
    {
        if (isStunned) return;

        isStunned = true;

        if (controller.DecreaseHP(dmg))
        {
            anim.SetTrigger("GetHit");

            HitDelay(0.7f).Forget();
        }
        else isStunned = false;

        async UniTaskVoid HitDelay(float duration)
        {
            isStunned = true;

            state.ChangeState(StateType.GetHit);

            await UniTask.Delay(System.TimeSpan.FromSeconds(duration), cancellationToken: source.Token);

            state.ChangeState(StateType.Move);

            isStunned = false;
        }
    }
    public override void PlayerUpdate()
    {
        state.StateUpdate();
    }

    public override void PlayerFixedUpdate()
    {
        state.FixedStateUpdate();

        if (!isGround)
        { // �÷��̾ ���� ���� �� isGround = true;
            if (Physics.Raycast(Position(), Vector3.down, 0.9f, 1 << 6)) isGround = true;
            Debug.DrawRay(Position(), Vector3.down * 0.9f, Color.red, 0.1f);
        }
    }

    

    public override bool IsDead() => controller.IsDead();
    public override bool IsSwitchable() => isGround;
    public override Vector3 Position() => this.transform.position;
    protected override void DecreaseStaminaValue(float value)
        => controller.DecreaseStaminaValue(value);
    public override bool IsAbleToUseSkill() => !isStunned;
    public override void UseSkill(SkillReferenceData skill, Action slotCallback, Action controllerCallback) { }
    public override void FindInteractableTarget() { } // ���ڷ� ź ���¿��� ��ȣ�ۿ� X
    public override void StartConversation(Transform target) { } // ���ڷ� ź ���¿��� ��ȣ�ۿ� X



}
