using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIdleState : MonsterState
{
    public MonsterIdleState(Monster _monster, MonsterStateController _controller) : base(_monster, _controller)
    {

    }

    private const float roamingDelayDuration = 5f;

    private float timer;

    public override void Entry()
    {
        timer = roamingDelayDuration;

        monster.Roam();
    }

    public override void Exit()
    {

    }

    public override void FixedStateUpdate()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = roamingDelayDuration;

            monster.Roam();
        }

        monster.Roaming();
    }

    public override void StateUpdate()
    {

    }
}
