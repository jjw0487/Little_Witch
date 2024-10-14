using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public abstract class MonsterState : FSM
{
    public MonsterState(Monster _monster, MonsterStateController _controller)
    {
        monster = _monster;
        stateController = _controller;
        lPlayer = 1 << 3;
    }

    protected int lPlayer;
    protected Monster monster;
    protected MonsterStateController stateController;

    public abstract void Entry();
    public abstract void Exit();
    public abstract void FixedStateUpdate();
    public abstract void StateUpdate();

}
