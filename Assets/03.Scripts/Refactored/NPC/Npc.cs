using Enums;
using UnityEngine;

public abstract class Npc : MonoBehaviour, INpc
{
    [SerializeField] protected NpcType type;
    [SerializeField] protected string playerPrefs;
    [SerializeField] protected GameObject notification;
    [SerializeField] protected DialogueData[] IdleDialogue;
    public abstract void InitializeNpc();
    public abstract void QuestNotification(bool active);
    public abstract void StartConversation(Transform target);

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IPlayer value))
        {
            if (EventManager.interactionNotificationEvent != null)
            {
                EventManager.interactionNotificationEvent(true);
            }
        }
          
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if(other.gameObject.TryGetComponent(out IPlayer value))
        {
            if (EventManager.interactionNotificationEvent != null)
            {
                EventManager.interactionNotificationEvent(false);
            }
        }
        
    }
}
