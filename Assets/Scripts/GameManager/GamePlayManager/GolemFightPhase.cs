using System;
using UnityEngine;

/// <summary>
/// 던전 내에서 순서대로 진행될 수 있는 Phase의 로직
/// </summary>
public class GolemFightPhase : DungeonScenePhase
{
    [SerializeField] private PhaseMonster monster; // 트리거가 발동하면 전투로 진행될 몬스터
    [SerializeField] private DialogueData dialogue; // 안내될 내용

    private bool isPhaseCleared = false; // Exit 후 다시 복귀할 때 

    public override void InitializePhase(Action _callback) // 이전 Phase가 완료되면 실행
    {
        base.InitializePhase(_callback);

        monster.Initialize(() =>
        {
            // death callback
            callback();
            isPhaseCleared = true;

            Debug.Log("Golem Fight Phase Cleared");
        });

        monster.Spawn(Vector3.zero); // 몬스터 소환
    }

    private void OnTriggerEnter(Collider other) // 플레이어가 범위 내로 진입할 시
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
