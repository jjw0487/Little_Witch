using Enums;
using UnityEngine;
using UnityEngine.EventSystems;
public abstract class NavMeshPlayerState : FSM
{
    
    public NavMeshPlayerState(NavMeshPlayer _player, NavMeshPlayerStateController _controller)
    {
        stateController = _controller;
        player = _player;

        lMonster = 1 << 8;
        lGround = 1 << 6;
        lSkillRange = 1 << 9;

        pointerUI = -1;
#if UNITY_ANDROID || UNITY_IOS
        pointerUI = 0;
#endif

    }

    protected NavMeshPlayerStateController stateController;

    protected NavMeshPlayer player;

    // 3 : player, 5 : UI, 6 : ground, 7 : obstacle, 8 : monster
    protected int lMonster;
    protected int lGround;
    protected int lSkillRange;

    protected int pointerUI;

    protected Ray ray;
    public abstract void Entry();
    public abstract void Exit();
    public abstract void StateUpdate();
    public abstract void FixedStateUpdate();

    
    

    protected virtual void Ray()
    {
        if (GetLayer(out RaycastHit hit, out LayerType type))
        {
            if (type == LayerType.Ground)
            {
                player.MoveRayPoint(hit);
            }
            else if (type == LayerType.Monster)
            {
                if (player.IsTargetAttackable(hit))
                {
                    if (hit.transform.parent.TryGetComponent(out IMonster value))
                    {
                        player.NormalAttack(value.HitPoint());
                    }
                }
                else
                {
                    if (hit.transform.parent.TryGetComponent(out IMonster value))
                    {
                        player.StepBeforeNormalAttack(value);
                    }
                    else
                    {
                        player.MoveRayPoint(hit);
                    }
                }
            }
        }
    }

    protected virtual bool GetLayer(out RaycastHit hit, out LayerType type)
    {
        if (EventSystem.current.
                IsPointerOverGameObject(pointerUI) == true)
        { //  UI 캔버스
            hit = new RaycastHit();
            type = LayerType.UI;
            return false;
        }

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 3f);

        if (Physics.Raycast(ray, out hit, 100f, lMonster))
        {
            type = LayerType.Monster;
            return true;
        }
        else if (Physics.Raycast(ray, out hit, 100f, lGround))
        {
            type = LayerType.Ground;
            return true;
        }

        type = LayerType.UI;
        return false;
    }
}
