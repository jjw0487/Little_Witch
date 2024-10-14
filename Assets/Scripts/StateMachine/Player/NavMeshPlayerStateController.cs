using Enums;
using System.Collections.Generic;

/// <summary>
/// Nav Mesh Agent Player State
/// </summary>
public class NavMeshPlayerStateController : StateController<NavMeshPlayerState>
{
    /// <summary>
    /// State 마다 실행할 코드를 구분함으로 불필요한 연산을 막음
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
