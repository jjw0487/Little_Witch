using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������� ����� ������� ����� �� �ֵ��� �ϱ� ����
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
