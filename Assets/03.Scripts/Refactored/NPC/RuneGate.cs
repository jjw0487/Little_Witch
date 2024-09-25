using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneGate : Npc
{
    [SerializeField] private string textLog;

    [SerializeField] private string moveSceneName;

    private void OnDisable()
    {
    }

    public override void InitializeNpc() { }

    public override void QuestNotification(bool active) { }

    public override void StartConversation(Transform target)
    {
        PlayManager.inst.Interact().StartConversation(IdleDialogue[0], this.transform, () =>
        {
            UIManager.inst.ShowAndGetPopup("TextLog", false)
            .GetComponent<TextLogPopup>().UIUpdate(textLog, () =>
            {
                SceneLoader.sInst.LoadScene(moveSceneName);
            });
        });
    }
}
