using Enums;
using System.Collections.Generic;

public class NavMeshPlayerStateController : StateController<NavMeshPlayerState>
{
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
