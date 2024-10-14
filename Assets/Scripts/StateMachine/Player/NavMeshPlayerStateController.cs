using Enums;
using System.Collections.Generic;

/// <summary>
/// Nav Mesh Agent Player State
/// </summary>
public class NavMeshPlayerStateController : StateController<NavMeshPlayerState>
{
    /// <summary>
    /// State ���� ������ �ڵ带 ���������� ���ʿ��� ������ ����
    /// </summary>
    /// <param name="p"></param>
    public NavMeshPlayerStateController(NavMeshPlayer p)
    {
        states = new Dictionary<StateType, NavMeshPlayerState>
        {
            { StateType.Init, new PlayerInitState(p, this) },
            { StateType.Idle, new PlayerIdleState(p, this) },
            { StateType.Move, new PlayerMoveState(p, this) },
            { StateType.Attack, new PlayerAttackState(p, this) },
            { StateType.GetHit, new PlayerGetHitState(p, this) },
            { StateType.Die, new PlayerDieState(p, this) }
        };

        InitializeState(StateType.Init);
    }
}
