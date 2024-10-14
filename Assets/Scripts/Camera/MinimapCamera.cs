using UnityEngine;

/// <summary>
/// 플레이어의 위치와 주변 기물을 확인할 수 있는 미니맵 카메라
/// </summary>
public class MinimapCamera : MonoBehaviour
{
    private PlayerController player;

    public void InitializeMinimapCamera(PlayerController _player)
    {
        player = _player;
    }

    private void LateUpdate()
    {
        Vector3 position = player.Position();

        position.y = this.transform.position.y;

        this.transform.position = position;
    }
}
