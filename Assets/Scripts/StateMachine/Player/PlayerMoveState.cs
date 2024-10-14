using UnityEngine;

public class PlayerMoveState : NavMeshPlayerState
{
    public PlayerMoveState(NavMeshPlayer _player,
        NavMeshPlayerStateController _controller) : base(_player, _controller) { }
    public override void Entry() { } // State 진입시 1회 실행
    public override void Exit() { } // State 탈출 시 1회 실행
    public override void FixedStateUpdate() // FixedUpdate()
    {
        player.Move();
    }

    public override void StateUpdate() // Update()
    {
        // 마우스 클릭 시 레이를 쏴 Monster 혹은 Ground를 검출하여 다음 행동을 정함
        if (Input.GetMouseButtonDown(0)) 
        {
            Ray();
        }
    }
}
