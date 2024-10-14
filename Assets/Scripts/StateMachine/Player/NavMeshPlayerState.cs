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
            // 전달 받은 값으로 다음 행동을 결정

            if (type == LayerType.Ground)
            {
                player.MoveRayPoint(hit); // 해당 위치까지 이동
            }
            else if (type == LayerType.Monster)
            {
                if (player.IsTargetAttackable(hit)) // 몬스터가 공격 가능한 위치일 시
                {
                    if (hit.transform.parent.TryGetComponent(out IMonster value))
                    {
                        player.NormalAttack(value.HitPoint()); // 일반 스킬 공격
                    }
                }
                else
                {
                    player.MoveRayPoint(hit); // 해당 위치까지 이동
                }
            }
        }
    }

    protected virtual bool GetLayer(out RaycastHit hit, out LayerType type)  
    {
        // 레이를 쏜 후 레이어를 검출하여 전달

        if (EventSystem.current.
                IsPointerOverGameObject(pointerUI) == true)
        { 
            // UI 캔버스 클릭하였을 시 무시
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
