using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panda : Npc
{
    private bool isFirstMeeting;

    private void OnDisable()
    {
        if(DataContainer.sInst != null) DataContainer.sInst.Quest().RemoveNpcListner(type);
    }

    private void Start()
    {
        InitializeNpc();
    }

    public override void InitializeNpc()
    {
        //isFirstMeeting = PLoad.Load(playerPrefs, true);

        if (DataContainer.sInst != null) DataContainer.sInst.Quest().AddNpcListner(type, this);
    }

    public override void QuestNotification(bool active)
    {
    }

    public override void StartConversation(Transform target)
    {
        PlayManager.inst.Interact().StartConversation(IdleDialogue[0], this.transform, () =>
        {
            UIManager.inst.ShowPopup("Store", true);
        });

        Quaternion dir = Quaternion.LookRotation((target.position - this.transform.position).normalized);
        transform.rotation = Quaternion.Euler(0f, dir.eulerAngles.y, 0f);
    }
}
