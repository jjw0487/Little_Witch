using UnityEngine;
using Enums;

public class PlayerIdleState : NavMeshPlayerState
{

    public PlayerIdleState(NavMeshPlayer _player, NavMeshPlayerStateController _controller) : base(_player, _controller)
    {

    }

    public override void Entry()
    {

    }

    public override void Exit()
    {

    }

    public override void FixedStateUpdate()
    {

    }

    public override void StateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray();
        }

    }

    protected override void Ray()
    {
        if (GetLayer(out RaycastHit hit, out LayerType type))
        {
            if (type == LayerType.Ground)
            {
                player.MoveRayPoint(hit);
                stateController.ChangeState(StateType.Move);
            }
            else if (type == LayerType.Monster)
            {
                if (player.IsTargetAttackable(hit))
                {
                    if (hit.transform.parent.TryGetComponent(out IMonster value))
                    {
                        player.NormalAttack(value.HitPoint());
                    }
                    else
                    {
                        player.MoveRayPoint(hit);
                    }
                }
            }
            else if (type == LayerType.Player)
            {
                // ĳ���Ͱ� ȣ���ǰ� �����̳� Ư���� UI�� �������� �غ���
            }

        }
    }
}
