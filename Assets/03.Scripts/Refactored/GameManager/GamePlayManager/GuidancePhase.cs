using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidancePhase : DungeonScenePhase
{
    [SerializeField] private BoxCollider trigger;
    [SerializeField] private DialogueData dialogue;

    [SerializeField] private Transform target;

    private void OnTriggerEnter(Collider other)
    {
        if (isPhaseEnabled)
        {
            if (other.TryGetComponent(out IPlayer value))
            {
                // 일회성 안내

                trigger.enabled = false;

                isPhaseEnabled = false;

                callback();

                PlayManager.inst.Interact().StartConversation(dialogue, target);

                Debug.Log("Guidance Phase Cleared");
            }
        }
    }
}
