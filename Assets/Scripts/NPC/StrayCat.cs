using UnityEngine;

public class StrayCat : Npc
{
    public override void InitializeNpc()
    {
    }

    public override void QuestNotification(bool active)
    {
    }

    public override void StartConversation(Transform target)
    {
        PlayManager.inst.Interact().StartConversation(IdleDialogue[0], this.transform);
    }
}
