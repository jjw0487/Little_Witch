using DG.Tweening;
using Enums;
using UnityEngine;
public class CameraMovement : MonoBehaviour
{
    // ī�޶� ������Ʈ�� �������� �þ� ���ع� ã�µ� ������ �ɸ��Ƿ� ī�޶󺸴� �ڿ��� ���̸� ���.
    [SerializeField] private Transform disturbingSightRayPoint;

    [SerializeField] private float lerpspeed;
    [SerializeField] private Vector3 navMeshPlayerOffset;
    [SerializeField] private Vector3 broomPlayerOffset;

    private PlayModeType playMode = PlayModeType.None;
    private PlayerController player;
    private DisturbingSightObject disturbingSightObj;

    private Ray ray;

    private int lDisturbingSight;
    private bool isDisturbing;
    
    public void InitializeFollowCamera(PlayerController _player, PlayModeType _playMode, Vector3 _pos)
    {
        lDisturbingSight = 1 << 16;
        isDisturbing = false;

        player = _player;

        playMode = _playMode;

        Vector3 offset = playMode == PlayModeType.Street ? navMeshPlayerOffset : broomPlayerOffset;

        this.transform.position = _pos + offset;

        LookAtPlayerBehind();
    }
    void LateUpdate() // ī�޶�
    {
        CameraLateUpdate();
    }

    public void ChangePlayMode(PlayModeType type) => playMode = type;

    public void CameraGetHit()
    { 
        this.transform.DOShakePosition(0.4f, 0.4f);
    }

    public void CameraInteractionAnimationOn()
    {
        LookAtPlayerBehind();

        float moveX = this.transform.position.y - 4f;

        Vector3 euler = this.transform.eulerAngles;
        euler.x = 20f;

        this.transform.DOMoveY(moveX, 0.7f);
        this.transform.DORotate(euler, 0.7f);
    }

    public void CameraInteractionAnimationOff()
    {
        Vector3 euler = this.transform.eulerAngles;
        euler.x = 40f;
        this.transform.DORotate(euler, 0.7f);
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            CameraGetHit();
        }
    }

    private void FixedUpdate()
    {
        if (playMode != PlayModeType.Broom) CheckIfDisturbingSight();
    }

    public void LookAtPlayerBehind(PlayModeType type = PlayModeType.Street)
    {
        if (type == PlayModeType.Street) LookAtNavmeshPlayerBehind();
        else LookAtBroomPlayerBehind();
    }

    private void LookAtNavmeshPlayerBehind()
    {
        // �÷��̾ �ٶ󺸴� ������ �������� ���� ��ġ ���
        Vector3 behindPosition = player.Position() - player.Transform().forward * 8f + Vector3.up * 8f; // Z������ -8, Y������ +8

        // ī�޶� ��ġ�� ��� �÷��̾� �ڷ� �̵�
        transform.position = behindPosition;

        // �÷��̾��� �ڿ��� �÷��̾ �ٶ󺸵��� ī�޶� ȸ�� ����, Y�� ȸ���� ����
        Quaternion targetRotation = Quaternion.LookRotation(player.Position() - transform.position);

        // X�� ȸ���� 40���� �����ϰ�, Y��� Z���� LookRotation���� ����
        Vector3 euler = targetRotation.eulerAngles; euler.x = 40f;
        transform.rotation = Quaternion.Euler(euler);

        navMeshPlayerOffset = transform.position - player.Position();
    }

    private void LookAtBroomPlayerBehind()
    {
        // �÷��̾ �ٶ󺸴� ������ �������� ���� ��ġ ���
        Vector3 behindPosition = player.Position() - player.Transform().forward * 4f + Vector3.up * 5f;

        // ī�޶� ��ġ�� ��� �÷��̾� �ڷ� �̵�
        transform.position = behindPosition;

        // �÷��̾��� �ڿ��� �÷��̾ �ٶ󺸵��� ī�޶� ȸ�� ����, Y�� ȸ���� ����
        Quaternion targetRotation = Quaternion.LookRotation(player.Position() - transform.position);

        // X�� ȸ���� 40���� �����ϰ�, Y��� Z���� LookRotation���� ����
        Vector3 euler = targetRotation.eulerAngles; euler.x = 40f;
        transform.rotation = Quaternion.Euler(euler);

        broomPlayerOffset = transform.position - player.Position();
    }


    public void CameraLateUpdate()
    {
        switch (playMode)
        {
            case PlayModeType.Interact: break;
            case PlayModeType.Street:

                transform.position = Vector3.Lerp(this.transform.position,
                    player.Position() + navMeshPlayerOffset, lerpspeed * Time.deltaTime);

                if (Input.GetMouseButton(1)) RotateNavMeshCamera();

                break;

            case PlayModeType.Broom:

                if (Input.GetMouseButton(1)) RotateBroomCamera();

                transform.position = player.Position() + broomPlayerOffset;

                //transform.position = Vector3.Lerp(this.transform.position,
                 //   player.Position() + broomPlayerOffset, lerpspeed * Time.deltaTime);

                break;
        }
    }

    void RotateNavMeshCamera()
    {
        // ��� �̵����Ѿ� ��Ȯ�� ��ġ�� Offset�� ������ �� ����
        transform.position = player.Position() + navMeshPlayerOffset;

        // �÷��̾� ��ġ���� ī�޶��� ���� ���� ���ϱ�
        Vector3 direction = transform.position - player.Position();

        // ȸ�� ����
        direction = Quaternion.Euler(0, Input.GetAxis("Mouse X") * 5f, 0) * direction;

        // ���ο� ��ġ ���
        transform.position = player.Position() + direction;

        // ī�޶� �÷��̾ �ٶ󺸰� ȸ��, Y�� ȸ���� ����
        Quaternion targetRotation = Quaternion.LookRotation(player.Position() - transform.position);

        // X�� ȸ���� 40���� �����ϰ�, Y��� Z���� LookRotation���� ����
        Vector3 euler = targetRotation.eulerAngles;
        euler.x = 40f;
        transform.rotation = Quaternion.Euler(euler);

        // ������ ������ ��ġ����ŭ ����
        navMeshPlayerOffset = new Vector3(direction.x, navMeshPlayerOffset.y, direction.z);
    }

    void RotateBroomCamera()
    {
        // ��� �̵����Ѿ� ��Ȯ�� ��ġ�� Offset�� ������ �� ����
        transform.position = player.Position() + broomPlayerOffset;

        // �÷��̾� ��ġ���� ī�޶��� ���� ���� ���ϱ�
        Vector3 direction = transform.position - player.Position();

        // ȸ�� ����
        direction = Quaternion.Euler(0, Input.GetAxis("Mouse X") * 5f, 0) * direction;

        // ���ο� ��ġ ���
        transform.position = player.Position() + direction;

        // ī�޶� �÷��̾ �ٶ󺸰� ȸ��, Y�� ȸ���� ����
        Quaternion targetRotation = Quaternion.LookRotation(player.Position() - transform.position);

        // X�� ȸ���� 40���� �����ϰ�, Y��� Z���� LookRotation���� ����
        Vector3 euler = targetRotation.eulerAngles;
        euler.x = 40f;
        transform.rotation = Quaternion.Euler(euler);

        // ������ ������ ��ġ����ŭ ����
        broomPlayerOffset = new Vector3(direction.x, broomPlayerOffset.y, direction.z);
    }

    private void CheckIfDisturbingSight()
    {
        // �þ߸� ���� ������ ������ �÷��̾ �������� �ش� �������� ��� ���д�.

        ray.origin = disturbingSightRayPoint.position;
        ray.direction = player.Position() - disturbingSightRayPoint.position;

        Debug.DrawRay(ray.origin, ray.direction * 50f, Color.blue, 0.4f);

        if (Physics.Raycast(ray, out RaycastHit hit, 50f, lDisturbingSight))
        {
            if (isDisturbing) return;

            if (hit.collider.gameObject.TryGetComponent(out DisturbingSightObject value))
            {
                isDisturbing = true;
                disturbingSightObj = value;
                disturbingSightObj.IsDisturbing(true);
            }
        }
        else
        {
            if (isDisturbing)
            {
                isDisturbing = false;
                disturbingSightObj.IsDisturbing(false);
            }
        }
    }
}