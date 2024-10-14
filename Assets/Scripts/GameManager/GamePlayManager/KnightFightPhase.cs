using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightFightPhase : DungeonScenePhase
{
    [SerializeField] private PhaseMonster[] monsters;
    [SerializeField] private DialogueData dialogue;

    private bool isPhaseCleared = false; // Exit �� �ٽ� ������ �� 
    private int deathCounter = 0;

    public override void InitializePhase(Action _callback)
    {
        base.InitializePhase(_callback);

        for(int i = 0; i < monsters.Length; i++)
        {
            monsters[i].Initialize(() =>
            {
                // death callback
                deathCounter++;

                if(deathCounter >= monsters.Length)
                {
                    callback();
                    isPhaseCleared = true;

                    Debug.Log("Knight Fight Phase Cleared");
                }
            });

            monsters[i].Spawn(Vector3.zero);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IPlayer value))
        {
            if (isPhaseEnabled)
            {
                isPhaseEnabled = false;

                PlayManager.inst.Interact().StartConversation(dialogue, monsters[0].transform, () => 
                {
                    for (int i = 0; i < monsters.Length; i++)
                    {
                        monsters[i].TargetOnManually(value);
                    }
                });
            }
            else
            {
                if (!isPhaseCleared) // ���� �ȿ� �����ٰ� �ٽ� ������ ���
                {
                    for (int i = 0; i < monsters.Length; i++)
                    {
                        monsters[i].TargetOnManually(value);
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out IPlayer value))
        {
            if (!isPhaseCleared) // Ŭ���� �Ǳ� ���� ���� ������ ������ ���
            {
                for (int i = 0; i < monsters.Length; i++)
                {
                    if(monsters[i].IsAlive())
                    {
                        monsters[i].LostTargetManually();
                    }
                }
            }
        }
    }
}
