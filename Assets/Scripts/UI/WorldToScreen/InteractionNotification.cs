using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionNotification : MonoBehaviour
{
    [SerializeField] private Transform interactionNotification;
    [SerializeField] private Animator anim;

    private bool isOn;
    public void Spawn(bool active)
    {
        if (isOn == active) return;

        isOn = active;

        if(isOn)
        {
            this.gameObject.SetActive(true);
            anim.SetBool("On", true);

            interactionNotification.DOMoveY(-100f, 0.3f).OnComplete(() => 
            {
                if (!isOn) Despawn();
            });
        }
        else
        {
            Despawn();
        }
    }

    private void Despawn()
    {
        interactionNotification.DOMoveY(0f, 0.3f).OnComplete(() =>
        {
            anim.SetBool("On", false);
            this.gameObject.SetActive(false);
        });
    }

}
