using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateController : StateController<MonsterState>
{
    public MonsterStateController(Monster _monster)
    {
        states = new Dictionary<StateType, MonsterState>
        {
            { StateType.Idle, new MonsterIdleState(_monster, this) },
            { StateType.Move, new MonsterMoveState(_monster, this) },
            { StateType.Attack, new MonsterAttackState(_monster, this) },
            { StateType.Pending, new MonsterPendingState(_monster,this) },
            { StateType.Die, new MonsterDieState(_monster,this) }
        };
    }
}