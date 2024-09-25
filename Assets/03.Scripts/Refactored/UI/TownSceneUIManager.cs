using DG.Tweening;
using System;
using UnityEngine;

public class TownSceneUIManager : UIManager
{
    [SerializeField] private Transform battleModeUI;
    [SerializeField] private Transform conversationModeUI;
    [SerializeField] private Transform minimapUI;

    [SerializeField] private QuickSlotManager quickSlotManager;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private PlayerStatusGaugeManager statusGaugeManager;

    private Action endConversationCallback;

    public override void InitializeUIManager(PlayerController player)
    {
        HideGUI();

        base.InitializeUIManager(player);

        statusGaugeManager.InitializePlayerStatusGauge(player);

        quickSlotManager.InitializeQuickSlotManager();
    }

    public override void HideGUI()
    {
        battleModeUI.DOMoveY(-500f, 0f);
        minimapUI.DOMoveX(-500f, 0f);

    }

    public override void ShowGUI()
    {
        battleModeUI.DOMoveY(0f, 0.6f);
        minimapUI.DOMoveX(50f, 0.6f);
    }



    public override void StartConversation(DialogueData dialogue, Action _callback = null)
    {
        endConversationCallback = _callback;

        battleModeUI.DOMoveY(-500f, 0.6f);
        conversationModeUI.DOMoveY(0f, 0.6f);

        dialogueManager.InitializeDialogue(dialogue, EndConversation);
    }

    public override void EndConversation()
    {
        PlayManager.inst.Interact().EndConversation();

        battleModeUI.DOMoveY(0f, 0.6f);
        conversationModeUI.DOMoveY(-500f, 0.6f);

        if(endConversationCallback != null) 
        {
            endConversationCallback();
            endConversationCallback = null;
        }
        
    }

    protected override void Update()
    {
        base.Update();
    }
    protected override void ESCPressed()
        => popupController.EscapePressed();
    public override GameObject GetDisposablePopup(string popup)
        => popupController.GetDisposablePopup(popup);
    public override void ShowPopup(string popupName, bool stack)
        => popupController.ShowPopup(popupName, stack);
    public override GameObject ShowAndGetPopup(string popupName, bool stack)
        => popupController.ShowAndGetPopup(popupName, stack);

    public override void ShortcutPopup()
    {
        ShowAndGetPopup("Shortcut", true).GetComponent<ShortcutPopup>().Init(inputManager.GetKeyEvents());
    }
}
