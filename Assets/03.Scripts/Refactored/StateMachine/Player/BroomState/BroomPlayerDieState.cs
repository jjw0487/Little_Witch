using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomPlayerDieState : BroomPlayerState
{
    public BroomPlayerDieState(BroomPlayer _player,
        BroomPlayerStateController _controller) : base(_player, _controller) { }

    public override void Entry() { }

    public override void Exit() { }

    public override void FixedStateUpdate() { }

    public override void StateUpdate() { }
}
