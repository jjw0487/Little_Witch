using UnityEngine;

public class PlayerMoveState : NavMeshPlayerState
{
    public PlayerMoveState(NavMeshPlayer _player,
        NavMeshPlayerStateController _controller) : base(_player, _controller) { }
    public override void Entry() { } // State ���Խ� 1ȸ ����
    public override void Exit() { } // State Ż�� �� 1ȸ ����
    public override void FixedStateUpdate() // FixedUpdate()
    {
        player.Move();
    }

    public override void StateUpdate() // Update()
    {
        // ���콺 Ŭ�� �� ���̸� �� Monster Ȥ�� Ground�� �����Ͽ� ���� �ൿ�� ����
        if (Input.GetMouseButtonDown(0)) 
        {
            Ray();
        }
    }
}
