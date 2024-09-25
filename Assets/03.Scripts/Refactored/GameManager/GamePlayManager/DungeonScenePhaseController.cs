using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 던전씬의 기믹이 순서대로 진행될 수 있도록 하기 위함
/// </summary>
public class DungeonScenePhaseController : MonoBehaviour
{
    [SerializeField] private DungeonScenePhase[] phases;

    private int phase = 0;

    private void Start()
    {
        phase = 0;

        phases[phase].InitializePhase(() => 
        {
            NextPhase();
        });
    }

    private void NextPhase()
    {
        if(phase < phases.Length - 1)
        {
            phase++;
            phases[phase].InitializePhase(NextPhase);
        }
    }
}
