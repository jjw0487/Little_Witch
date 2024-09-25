public class MonsterDieState : MonsterState
{
    public MonsterDieState(Monster _monster, MonsterStateController _controller) : base(_monster, _controller)
    {

    }

    public override void Entry()
    {
        monster.Die();
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
