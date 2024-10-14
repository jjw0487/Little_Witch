using UnityEngine;

public interface INpc
{
    void InitializeNpc();
    void StartConversation(Transform target);
    void QuestNotification(bool active);
}
