using Enums;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAttackState : NavMeshPlayerState
{
    public PlayerAttackState(NavMeshPlayer _player, NavMeshPlayerStateController _controller) : base(_player, _controller)
    {

    }

    public override void Entry()
    {
     
    }

    public override void Exit()
    {
      
    }

    public override void FixedStateUpdate()
    {
    
    }

    public override void StateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray();
        }
    }

    protected override void Ray()
    {
        if (GetLayer(out RaycastHit hit, out LayerType type))
        {
            if (type == LayerType.SkillRange)
            {
                player.SkillRayPoint(hit);
            }
            else
            {
                player.CancelUsingSkill();
            }
        }
    }

    protected override bool GetLayer(out RaycastHit hit, out LayerType type)
    {
        if (EventSystem.current.
                IsPointerOverGameObject(pointerUI) == true)
        { //  UI Äµ¹ö½º
            hit = new RaycastHit();
            type = LayerType.UI;
            return false;
        }

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red, 10f);

        if (Physics.Raycast(ray, out hit, 1000f, lSkillRange))
        {
            type = LayerType.SkillRange;
            return true;
        }

        type = LayerType.UI;
        return true;
    }
}
