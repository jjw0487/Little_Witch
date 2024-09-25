using UnityEngine;

public class DropStonePhase : DungeonScenePhase
{
    [SerializeField] private BoxCollider trigger;
    [SerializeField] private GameObject stones;
    [SerializeField] private Vector3 dropPosition;
    [SerializeField] private DialogueData dialogue;

    private void OnTriggerEnter(Collider other)
    {
        if(isPhaseEnabled)
        {
            if (other.TryGetComponent(out IPlayer value))
            {
                // 일회성 장애물
                var obj = Instantiate(stones, dropPosition, Quaternion.identity);

                trigger.enabled = false;

                isPhaseEnabled = false;

                callback();

                PlayManager.inst.Interact().StartConversation(dialogue, obj.transform);

                Debug.Log("Drop Stones Phase Cleared");
            }
        }
    }
}
