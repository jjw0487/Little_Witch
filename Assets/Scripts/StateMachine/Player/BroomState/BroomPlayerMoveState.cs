public class BroomPlayerMoveState : BroomPlayerState
{
    public BroomPlayerMoveState(BroomPlayer _player,
        BroomPlayerStateController _controller) : base(_player, _controller) { }

    public override void Entry() { }

    public override void Exit() { }

    public override void FixedStateUpdate()
    {
        player.Move();
        player.TakeOff();
    }

    public override void StateUpdate() 
    {
        
    }
}
