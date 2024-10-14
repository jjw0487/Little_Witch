using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIManager : MonoBehaviour
{
    public static UIManager inst;
    private void Awake() => inst = this;

    [SerializeField] private List<GamePopup> popupList;

    [SerializeField] private Transform popupCanvas;
    [SerializeField] protected InputManager inputManager;

    protected PopupList popups;
    protected PopupController popupController;

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ESCPressed();
        }
    }

    public virtual void InitializeUIManager(PlayerController player)
    {
        if (popupController == null)
        {
            popups = new PopupList(popupList);
            popupController = new PopupController(popupCanvas, popups);
        }
    }

    public void ForceInitialize()
    {
        if (popupController == null)
        {
            popups = new PopupList(popupList);
            popupController = new PopupController(popupCanvas, popups);
        }
    }

    protected abstract void ESCPressed();
    public abstract void ShowPopup(string popupName, bool stack);
    public abstract GameObject ShowAndGetPopup(string popupName, bool stack);
    public abstract GameObject GetDisposablePopup(string popup);
    public abstract void StartConversation(DialogueData dialogue, Action _callback = null);
    public abstract void EndConversation();
    public abstract void ShortcutPopup();
    public abstract void HideGUI();
    public abstract void ShowGUI();
}
