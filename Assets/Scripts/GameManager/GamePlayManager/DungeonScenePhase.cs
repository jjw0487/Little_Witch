using System;
using UnityEngine;

/// <summary>
/// GuidancePhase.cs, DropStonePhase.cs, KnightFightPhase.cs, GolemFightPhase.cs, BossFightPhase.cs�� phase�� �ֽ��ϴ�.
/// 
/// ���� ������ ������� ����� �� �ִ� Phase�� ����
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
