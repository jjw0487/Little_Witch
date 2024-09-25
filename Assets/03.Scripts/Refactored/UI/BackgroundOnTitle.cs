using UnityEngine;

public class BackgroundOnTitle : MonoBehaviour
{
    public float speed;

    [SerializeField] private Renderer BGs;


    void Update()
    {
        BGs.material.mainTextureOffset += new Vector2(speed * Time.deltaTime, 0);
    }
}
