using UnityEngine;
/// <summary>
/// Ÿ��Ʋ �� ȭ���� ����� ���͸����� ������ ������ �帧�� ǥ�� 
/// </summary>
public class BackgroundOnTitle : MonoBehaviour
{
    [SerializeField] private float speed; // �̵� �ӵ�
    [SerializeField] private Renderer BGs; // ������ ���

    void Update()
    {
        BGs.material.mainTextureOffset += new Vector2(speed * Time.deltaTime, 0);
    }
}
