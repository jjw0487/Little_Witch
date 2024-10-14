using UnityEngine;
/// <summary>
/// 타이틀 씬 화면의 배경의 메터리얼을 움직여 구름의 흐름을 표현 
/// </summary>
public class BackgroundOnTitle : MonoBehaviour
{
    [SerializeField] private float speed; // 이동 속도
    [SerializeField] private Renderer BGs; // 움직일 대상

    void Update()
    {
        BGs.material.mainTextureOffset += new Vector2(speed * Time.deltaTime, 0);
    }
}
