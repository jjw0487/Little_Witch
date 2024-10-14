using System;
using System.Collections;
using UnityEngine;

public class InteractionManager
{
    public InteractionManager(PlayerController _player, UIManager _uiManager, CameraMovement _camera)
    {
        player = _player;
        camera = _camera;
        uiManager = _uiManager;
    }

    private PlayerController player;
    private CameraMovement camera;
    private UIManager uiManager;

    public void StartConversation(DialogueData dialogue, Transform target, Action callback = null)
    {
        player.StartConversation(target);
        uiManager.StartConversation(dialogue, callback);

        if (EventManager.interactionNotificationEvent != null)
        {
            EventManager.interactionNotificationEvent(false);
        }
    }

    public void EndConversation()
    {
        player.EndConversation();
    }
}
