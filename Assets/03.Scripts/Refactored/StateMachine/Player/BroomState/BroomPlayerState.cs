public abstract class BroomPlayerState : FSM
{
    public BroomPlayerState(BroomPlayer _player, BroomPlayerStateController _controller)
    {
        stateController = _controller;
        player = _player;

    }
    protected BroomPlayerStateController stateController;

    protected BroomPlayer player;

    public abstract void Entry();
    public abstract void Exit();
    public abstract void StateUpdate();
    public abstract void FixedStateUpdate();
}
