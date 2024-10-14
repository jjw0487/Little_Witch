using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPendingState : MonsterState
{
    public MonsterPendingState(Monster _monster, MonsterStateController _controller)
        : base(_monster, _controller) { }
    

    public override void Entry()
    {
        // Do nothing, just wait for the next state
    }

    public override void Exit() { }
    public override void FixedStateUpdate() { }
    public override void StateUpdate() { }
}
