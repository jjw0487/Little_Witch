using Enums;
using System.Collections.Generic;

public class BroomPlayerStateController : StateController<BroomPlayerState>
{
    public BroomPlayerStateController(BroomPlayer p)
    {
        states = new Dictionary<StateType, BroomPlayerState>
        {
            { StateType.Move, new BroomPlayerMoveState(p, this) },
            { StateType.Attack, new BroomPlayerAttackState(p, this) },
            { StateType.GetHit, new BroomPlayerGetHitState(p, this) },
            { StateType.Die, new BroomPlayerDieState(p, this) }
        };

        InitializeState(StateType.Move);
    }
}
