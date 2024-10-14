using System;
using DG.Tweening;
using UnityEngine;

public abstract class IPopup : MonoBehaviour
{
    [SerializeField] protected Transform tr_Body;

    protected bool isTweening = false;
    protected Vector2 orgPos;

    protected void Awake() => orgPos = tr_Body.localPosition;
    protected void SlideIn(Action act = null)
    {
        if (isTweening) return;

        isTweening = true;

        tr_Body.localPosition = new Vector2(orgPos.x - 25f, orgPos.y);

        tr_Body.DOLocalMoveX(2000f, 0.4f).From().OnComplete(() =>
        {
            tr_Body.DOLocalMoveX(orgPos.x, 0.2f).OnComplete(() =>
            {
                if (act != null) act();
            });

            isTweening = false;
        });

        SoundManager.sInst.Play("Popup");
    }

    protected void SlideOut(Action act = null)
    {
        isTweening = true;

        OnButtonClick();

        tr_Body.DOLocalMoveX(-2000f, 0.3f).OnComplete(() =>
        {
            if (act != null) act();

            isTweening = false;
        });
    }

    public abstract void UIUpdate();
    public abstract void EscapePressed();
    public abstract void Exit();

    public virtual void OnButtonClick()
    {
        SoundManager.sInst.Play("ButtonClick");
    }
}
