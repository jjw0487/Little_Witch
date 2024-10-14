using Enums;
using System.Collections.Generic;

/// <summary>
/// FSM 구조 중앙 처리
/// </summary>
/// <typeparam name="State"></typeparam>
public class StateController<State> where State : FSM
{
    protected State curState;
    protected State prevState;

    protected StateType curType;
    protected StateType prevType;

    public Dictionary<StateType, State> states;

    public StateType GetStateType() => curType;

    public void InitializeState(StateType type)
    {
        if (states.TryGetValue(type, out State stateType))
        {
            curState = stateType;
            curState.Entry();
        }
        else UnityEngine.Debug.Log(type + " 없는데? 코드 다시해 ");
    }


    public void ChangeState(StateType newType)
    {
        if (curType == newType)
        {
            return;
        }

        if (states.TryGetValue(newType, out State newState))
        {
            TransitionToState(newState, newType);
        }
        else UnityEngine.Debug.Log(newType + " 없는데? 코드 다시해 ");
    }

    public void StateUpdate()
    {
        // FSM 에서 업데이트에서 처리하고 싶은 부분이 있으면
        curState?.StateUpdate();
    }

    public void FixedStateUpdate()
    {
        // FSM 에서 Fixed 업데이트에서 처리하고 싶은 부분이 있으면
        curState?.FixedStateUpdate();
    }

    private void TransitionToState(State newState, StateType newType) // To new state
    {
        curState?.Exit();

        prevState = curState;
        prevType = curType;

        curState = newState;
        curType = newType;

        curState.Entry();
    }
}
