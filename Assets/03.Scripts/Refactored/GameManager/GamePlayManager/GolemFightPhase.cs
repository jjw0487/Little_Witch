using System;
using UnityEngine;

public class GolemFightPhase : DungeonScenePhase
{
    [SerializeField] private PhaseMonster monster;
    [SerializeField] private DialogueData dialogue;

    private bool isPhaseCleared = false; // Exit 후 다시 복귀할 때 

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
                if (!isPhaseCleared) // 범위 안에 나갔다가 다시 들어왔을 경우
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
            if (!isPhaseCleared) // 클리어 되기 전에 범위 밖으로 나갔을 경우
            {
                monster.LostTargetManually();
            }
        }
    }
}
