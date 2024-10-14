using System;
using UnityEngine;

/// <summary>
/// GuidancePhase.cs, DropStonePhase.cs, KnightFightPhase.cs, GolemFightPhase.cs, BossFightPhase.cs의 phase가 있습니다.
/// 
/// 던전 내에서 순서대로 진행될 수 있는 Phase의 로직
/// </summary>
public class DungeonScenePhase : MonoBehaviour
{
    protected Action callback;
    protected bool isPhaseEnabled = false;

    public virtual void InitializePhase(Action _callback)
    {
        isPhaseEnabled = true;
        callback = _callback;
    }
}
