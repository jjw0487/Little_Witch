using System;
using UnityEngine;

public class GolemFightPhase : DungeonScenePhase
{
    [SerializeField] private PhaseMonster monster;
    [SerializeField] private DialogueData dialogue;

    private bool isPhaseCleared = false; // Exit �� �ٽ� ������ �� 

    public override void InitializePhase(Action _callback)
    {
        base.InitializePhase(_callback);

        monster.Initialize(() =>
        {
            // death callback

            callback();
            isPhaseCleared = true;

            Debug.Log("Golem Fight Phase Cleared");
        });

        monster.Spawn(Vector3.zero);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IPlayer value))
        {
            if (isPhaseEnabled)
            {
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
