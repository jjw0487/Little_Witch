using UnityEngine;

[CreateAssetMenu(fileName = "PlayerOnBroomMovementDataConfig", menuName =
        "ScriptableObject/PlayerOnBroomMovementDataConfig", order = 1)]
public class PlayerOnBroomMovementDataConfig : MovementDataConfig
{
    public float restricted_Flying_Height = 1000;
}
