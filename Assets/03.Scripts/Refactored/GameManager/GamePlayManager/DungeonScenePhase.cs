using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
