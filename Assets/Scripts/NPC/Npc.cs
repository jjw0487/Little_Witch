using Enums;
using UnityEngine;

/// <summary>
/// NPC���� ����� �ൿ �Լ�
/// </summary>
public abstract class Npc : MonoBehaviour, INpc
{
    [SerializeField] protected NpcType type; 
    [SerializeField] protected string playerPrefs; // ó�� ���� Npc���� Ȯ��
    [SerializeField] protected GameObject notification; // ���� ������ ����Ʈ�� ������ ����
    [SerializeField] protected DialogueData[] IdleDialogue; // �⺻ ��ȭ
    public abstract void InitializeNpc();
    public abstract void QuestNotification(bool active);
    public abstract void StartConversation(Transform target);

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IPlayer value))
        {// �÷��̾ ����Ű�� ������ ��ȭ�� ������ ��ġ�� �ִٸ� ǥ��
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
