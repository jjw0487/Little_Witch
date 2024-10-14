using Enums;
using UnityEngine;

/// <summary>
/// NPC들의 공통된 행동 함수
/// </summary>
public abstract class Npc : MonoBehaviour, INpc
{
    [SerializeField] protected NpcType type; 
    [SerializeField] protected string playerPrefs; // 처음 만난 Npc인지 확인
    [SerializeField] protected GameObject notification; // 진행 가능한 퀘스트가 있으면 노출
    [SerializeField] protected DialogueData[] IdleDialogue; // 기본 대화
    public abstract void InitializeNpc();
    public abstract void QuestNotification(bool active);
    public abstract void StartConversation(Transform target);

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IPlayer value))
        {// 플레이어가 단축키를 누르면 대화가 가능한 위치에 있다면 표시
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
