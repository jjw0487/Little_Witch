using UnityEngine;

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
