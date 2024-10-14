using DG.Tweening;
using Enums;
using UnityEngine;
public class CameraMovement : MonoBehaviour
{
    // 카메라가 오브젝트에 포개지면 시야 방해물 찾는데 제약이 걸리므로 카메라보다 뒤에서 레이를 쏜다.
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
    void LateUpdate() // 카메라
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
        // 플레이어가 바라보는 방향을 기준으로 뒤쪽 위치 계산
        Vector3 behindPosition = player.Position() - player.Transform().forward * 8f + Vector3.up * 8f; // Z축으로 -8, Y축으로 +8

        // 카메라 위치를 즉시 플레이어 뒤로 이동
        transform.position = behindPosition;

        // 플레이어의 뒤에서 플레이어를 바라보도록 카메라 회전 설정, Y축 회전만 적용
        Quaternion targetRotation = Quaternion.LookRotation(player.Position() - transform.position);

        // X축 회전을 40도로 고정하고, Y축과 Z축은 LookRotation으로 설정
        Vector3 euler = targetRotation.eulerAngles; euler.x = 40f;
        transform.rotation = Quaternion.Euler(euler);

        navMeshPlayerOffset = transform.position - player.Position();
    }

    private void LookAtBroomPlayerBehind()
    {
        // 플레이어가 바라보는 방향을 기준으로 뒤쪽 위치 계산
        Vector3 behindPosition = player.Position() - player.Transform().forward * 4f + Vector3.up * 5f;

        // 카메라 위치를 즉시 플레이어 뒤로 이동
        transform.position = behindPosition;

        // 플레이어의 뒤에서 플레이어를 바라보도록 카메라 회전 설정, Y축 회전만 적용
        Quaternion targetRotation = Quaternion.LookRotation(player.Position() - transform.position);

        // X축 회전을 40도로 고정하고, Y축과 Z축은 LookRotation으로 설정
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
        // 즉시 이동시켜야 정확한 수치로 Offset을 변경할 수 있음
        transform.position = player.Position() + navMeshPlayerOffset;

        // 플레이어 위치에서 카메라의 방향 벡터 구하기
        Vector3 direction = transform.position - player.Position();

        // 회전 적용
        direction = Quaternion.Euler(0, Input.GetAxis("Mouse X") * 5f, 0) * direction;

        // 새로운 위치 계산
        transform.position = player.Position() + direction;

        // 카메라가 플레이어를 바라보게 회전, Y축 회전만 적용
        Quaternion targetRotation = Quaternion.LookRotation(player.Position() - transform.position);

        // X축 회전을 40도로 고정하고, Y축과 Z축은 LookRotation으로 설정
        Vector3 euler = targetRotation.eulerAngles;
        euler.x = 40f;
        transform.rotation = Quaternion.Euler(euler);

        // 오프셋 수정된 위치값만큼 저장
        navMeshPlayerOffset = new Vector3(direction.x, navMeshPlayerOffset.y, direction.z);
    }

    void RotateBroomCamera()
    {
        // 즉시 이동시켜야 정확한 수치로 Offset을 변경할 수 있음
        transform.position = player.Position() + broomPlayerOffset;

        // 플레이어 위치에서 카메라의 방향 벡터 구하기
        Vector3 direction = transform.position - player.Position();

        // 회전 적용
        direction = Quaternion.Euler(0, Input.GetAxis("Mouse X") * 5f, 0) * direction;

        // 새로운 위치 계산
        transform.position = player.Position() + direction;

        // 카메라가 플레이어를 바라보게 회전, Y축 회전만 적용
        Quaternion targetRotation = Quaternion.LookRotation(player.Position() - transform.position);

        // X축 회전을 40도로 고정하고, Y축과 Z축은 LookRotation으로 설정
        Vector3 euler = targetRotation.eulerAngles;
        euler.x = 40f;
        transform.rotation = Quaternion.Euler(euler);

        // 오프셋 수정된 위치값만큼 저장
        broomPlayerOffset = new Vector3(direction.x, broomPlayerOffset.y, direction.z);
    }

    private void CheckIfDisturbingSight()
    {
        // 시야를 막는 구조물 때문에 플레이어가 가려지면 해당 구조물을 잠시 꺼둔다.

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