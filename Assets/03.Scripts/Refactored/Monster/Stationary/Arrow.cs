using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private ParticleSystem trail;
    [SerializeField] private float speed = 15f;
    [SerializeField] private float gravity = -9.8f; // �߷� ���ӵ� (���� ������ ����)

    private bool isOnFire = false;
    private Vector3 targetDirection;
    private float verticalVelocity = 0f; // ���� ���� �ӵ�



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
            // ���� �ӵ��� �߷� �ݿ�
            verticalVelocity += gravity * Time.deltaTime;

            // ȭ���� �̵�
            Vector3 movement = targetDirection * speed * Time.deltaTime;

            // �߷����� ���� ���� ���� �̵� �ݿ�
            movement.y += verticalVelocity * Time.deltaTime;

            // ȭ���� ���ο� ��ġ ����
            transform.position += movement;
        }
    }

    public void Despawn()
    {
        trail.Stop();
        this.gameObject.SetActive(false);
    }
}
