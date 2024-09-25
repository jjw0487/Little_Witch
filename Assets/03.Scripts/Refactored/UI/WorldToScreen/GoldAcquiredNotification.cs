using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldAcquiredNotification : MonoBehaviour
{
    [SerializeField] private Text txt_Desc;

    private bool isOn = false;

    public bool IsOn() => isOn;

    public void Spawn(int value)
    {
        isOn = true;

        if(value > 0) txt_Desc.text = $"{value} Gold Acquired!";
        else txt_Desc.text = $"{value} Gold Consumed!";

        this.transform.SetAsFirstSibling();

        this.gameObject.SetActive(true);

        this.transform.DOScaleY(1f, 0.3f).OnComplete(() =>
        {
            this.transform.DOScaleY(1f, 1.5f).OnComplete(() =>
            {
                this.transform.DOScaleY(0.1f, 0.3f).OnComplete(() => { Despawn(); });
            });
        });
    }

    public void Despawn()
    {
        isOn = false;

        this.gameObject.SetActive(false);
    }
}
