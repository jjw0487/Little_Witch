using Enums;

public class PlayerInitState : NavMeshPlayerState
{

    public PlayerInitState(NavMeshPlayer _player, NavMeshPlayerStateController _controller) : base(_player, _controller)
    {

    }

    public override void Entry()
    {
        stateController.ChangeState(StateType.Idle);
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
