using Enums;
using System.Collections.Generic;

/// <summary>
/// FSM ���� �߾� ó��
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
        else UnityEngine.Debug.Log(type + " ���µ�? �ڵ� �ٽ��� ");
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
        else UnityEngine.Debug.Log(newType + " ���µ�? �ڵ� �ٽ��� ");
    }

    public void StateUpdate()
    {
        // FSM ���� ������Ʈ���� ó���ϰ� ���� �κ��� ������
        curState?.StateUpdate();
    }

    public void FixedStateUpdate()
    {
        // FSM ���� Fixed ������Ʈ���� ó���ϰ� ���� �κ��� ������
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
