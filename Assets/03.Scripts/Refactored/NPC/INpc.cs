using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INpc
{
    void InitializeNpc();
    void StartConversation(Transform target);
    void QuestNotification(bool active);
}
