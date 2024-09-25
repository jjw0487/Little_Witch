using Enums;
using UnityEngine;


public class PlayerMoveState : NavMeshPlayerState
{

    public PlayerMoveState(NavMeshPlayer _player,
        NavMeshPlayerStateController _controller) : base(_player, _controller) { }
    public override void Entry() { }
    public override void Exit() { }
    public override void FixedStateUpdate()
    {
        player.Move();
    }

    public override void StateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray();
        }
    }
}
