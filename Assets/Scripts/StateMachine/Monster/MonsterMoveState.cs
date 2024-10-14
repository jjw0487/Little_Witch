public class MonsterMoveState : MonsterState
{
    public MonsterMoveState(Monster _monster, MonsterStateController _controller) : base(_monster, _controller)
    {

    }

    public override void Entry()
    {
        monster.Move();
    }

    public override void Exit()
    {

    }

    public override void FixedStateUpdate()
    {
        monster.FollowTarget();
    }

    public override void StateUpdate()
    {

    }
}
