using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private ParticleSystem trail;
    [SerializeField] private float speed = 15f;
    [SerializeField] private float gravity = -9.8f; // 중력 가속도 (음수 값으로 설정)

    private bool isOnFire = false;
    private Vector3 targetDirection;
    private float verticalVelocity = 0f; // 수직 방향 속도



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IPlayer value))
        {
            isOnFire = false;

            value.GetHit(50f);

            Despawn();
        }
    }

    public void OnFire(Vector3 startPos, Vector3 direction)
    {
        this.gameObject.SetActive(true);

        this.transform.position = startPos;

        direction.y += 1f;

        targetDirection = (direction - startPos).normalized;

        verticalVelocity = 0f;

        isOnFire = true;

        trail.Play();
    }

    private void FixedUpdate()
    {
        if(isOnFire)
        {
            // 수직 속도에 중력 반영
            verticalVelocity += gravity * Time.deltaTime;

            // 화살의 이동
            Vector3 movement = targetDirection * speed * Time.deltaTime;

            // 중력으로 인한 수직 방향 이동 반영
            movement.y += verticalVelocity * Time.deltaTime;

            // 화살의 새로운 위치 적용
            transform.position += movement;
        }
    }

    public void Despawn()
    {
        trail.Stop();
        this.gameObject.SetActive(false);
    }
}
