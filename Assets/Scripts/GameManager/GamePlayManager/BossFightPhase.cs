using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightPhase : DungeonScenePhase
{
    [SerializeField] private AudioClip encounter;
    [SerializeField] private PhaseMonster monster;
    [SerializeField] private DialogueData dialogue;
    [SerializeField] private SecurityBot[] bots;

    private bool isPhaseCleared = false; // Exit �� �ٽ� ������ �� 

    public override void InitializePhase(Action _callback)
    {
        base.InitializePhase(_callback);

        monster.Initialize(() =>
        {
            // death callback

            callback();
            isPhaseCleared = true;

            for(int i = 0; i < bots.Length; i++) 
            {
                bots[i].StopMachine();
            }

            Debug.Log("Boss Fight Phase Cleared");
        });

        monster.Spawn(Vector3.zero);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IPlayer value))
        {
            if (isPhaseEnabled)
            {
                SoundManager.sInst.Play(encounter);
                isPhaseEnabled = false;

                PlayManager.inst.Interact().StartConversation(dialogue, monster.transform, () =>
                {
                    monster.TargetOnManually(value);
                });
            }
            else
            {
                if (!isPhaseCleared) // ���� �ȿ� �����ٰ� �ٽ� ������ ���
                {
                    monster.TargetOnManually(value);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IPlayer value))
        {
            if (!isPhaseCleared) // Ŭ���� �Ǳ� ���� ���� ������ ������ ���
            {
                monster.LostTargetManually();
            }
        }
    }
}
