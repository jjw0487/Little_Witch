using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class QuestNotification : MonoBehaviour
{
    protected bool isOn = false;
    public bool IsOn() => isOn;

    public virtual void Spawn(QuestData quest)
    {     
        this.transform.SetAsFirstSibling();

        this.gameObject.SetActive(true);

        this.transform.DOScaleY(1f, 0.5f).OnComplete(() =>
        {
            this.transform.DOScaleY(1f, 4f).OnComplete(() =>
            {
                this.transform.DOScaleY(0.1f, 0.5f).OnComplete(() => { Despawn(); });
            });
        });
    }

    public void Despawn()
    {
        isOn = false;

        this.gameObject.SetActive(false);
    }
}
