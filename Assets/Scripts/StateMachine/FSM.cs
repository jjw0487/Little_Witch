public interface FSM
{
    void Entry();
    void StateUpdate();
    void FixedStateUpdate();
    void Exit();
}