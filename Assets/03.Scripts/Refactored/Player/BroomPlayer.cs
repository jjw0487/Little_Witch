using Cysharp.Threading.Tasks;
using Enums;
using System;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class BroomPlayer : Player
{
    [SerializeField] protected PlayerOnBroomMovementDataConfig data;
    [SerializeField] private Rigidbody rb;

    [SerializeField] private Transform attackPoint;

    [SerializeField] protected GameObject orgDashEffect;

    private Vector3 dir = Vector3.zero;

    private PlayerController controller;

    private BroomPlayerStateController state;

    private Camera cam;

    private bool isStunned = false;

    private bool isGround = true;

    public override bool IsDead() => controller.IsDead();
    public override bool IsSwitchable() => isGround;
    public override Vector3 Position() => this.transform.position;

    protected override void DecreaseStaminaValue(float value)
        => controller.DecreaseStaminaValue(value);
    public override void InitializeMovement(PlayerController _controller)
    {
        controller = _controller;

        cam = Camera.main;

        state = new BroomPlayerStateController(this);

        rb.drag = 6f;
        rb.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
    }

    public override void Die()
    {

    }

    public override void GetHit(float dmg)
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

    protected void Update()
    {
        state.StateUpdate();
    }

    protected void FixedUpdate()
    {
        state.FixedStateUpdate();

        if(!isGround)
        {

            if (Physics.Raycast(Position(), Vector3.down, out RaycastHit hit, 0.9f, 1<<6))
            {
                // 닿은 물체의 이름을 출력
                Debug.Log(hit.collider.gameObject.name);

                isGround = true;
            }

            Debug.DrawRay(Position(), Vector3.down * 0.9f, Color.red, 0.1f);

            
        }
    }

    public void TakeOff()
    {
        if (controller.GetStamina() < 0.1f) return;

        /*if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Vector3 dashPower = this.transform.forward * -Mathf.Log(1 / rb.drag) * 6f;

            rb.AddForce(dashPower, ForceMode.VelocityChange);

            Instantiate(orgDashEffect, this.transform.position, this.transform.rotation);

            DecreaseStaminaValue(5f);
        }*/

        if (this.transform.position.y < data.restricted_Flying_Height)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Vector3 jumpPower = Vector3.up * 0.3f;

                rb.AddForce(jumpPower, ForceMode.VelocityChange);

                DecreaseStaminaValue(0.1f);

                if(isGround)
                {
                    isGround = false;
                }
            }
        }
    }

    public void Move()
    {
        dir.Set(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        dir = cam.transform.rotation * dir;
        dir.y = 0f;
        dir.Normalize();

        float speed = isGround ? 2f : data.speed;

        rb.MovePosition(this.transform.position + dir * speed * Time.deltaTime);

        if (dir != Vector3.zero)
        {
            // 앞으로 나아갈 때 + 방향으로 나아가는데 반대방향으로 나가가는 키를 눌렀을 때 -방향으로 회전하면서 생기는 오류를 방지하기위해 (부호가 서로 반대일 경우를 체크해서 살짝만 미리 돌려주는 코드) 어렵네요... 
            // 지금 바라보는 방향의 부호 != 나아갈 방향 부호
            if (Mathf.Sign(transform.forward.x) != Mathf.Sign(dir.x) ||
                Mathf.Sign(transform.forward.z) != Mathf.Sign(dir.z))
            {
                //우리는 이동할 때 x 와 z 밖에 사용을 안하므로
                transform.Rotate(0, 1, 0); // 살짝만 회전
                                           //정 반대방향을 눌러도 회전안하는 버그 방지
                                           //미리 회전을 조금 시켜서 정반대인 경우를 제거
            }

            transform.forward = Vector3.Lerp(transform.forward, dir, data.rotSpeed * Time.deltaTime);
            // 캐릭터의 앞방향은 dir 키보드를 누른 방향으로 캐릭터 회전

        }
        //transform.forward = Vector3.Lerp(transform.forward, dir, B_RotSpeed * Time.deltaTime);

    }

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

    public override void UseSkill(SkillReferenceData skill, Action slotCallback, Action controllerCallback)
    {
        
    }
    public override bool IsAbleToUseSkill() => !isStunned;

    public override void FindInteractableTarget() { } // 빗자루 탄 상태에서 상호작용? 고민해보자
    public override void StartConversation(Transform target) { } // 빗자루 탄 상태에서 상호작용? 고민해보자

    public override void PlayerUpdate()
    {
        state.StateUpdate();
    }

    public override void PlayerFixedUpdate()
    {
        state.FixedStateUpdate();
    }

}
