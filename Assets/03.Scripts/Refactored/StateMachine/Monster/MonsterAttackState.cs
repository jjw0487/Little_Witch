using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackState : MonsterState
{
    public MonsterAttackState(Monster _monster, MonsterStateController _controller) : base(_monster, _controller)
    {

    }

    public override void Entry()
    {
        monster.Attack();
    }

    public override void Exit()
    {

    }

    public override void FixedStateUpdate()
    {

    }

    public override void StateUpdate()
    {

    }
}
