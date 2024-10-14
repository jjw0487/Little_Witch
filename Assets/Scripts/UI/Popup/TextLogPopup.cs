using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class TextLogPopup : IPopup
{
    [SerializeField] private Button btn_Yes;
    [SerializeField] private Button btn_No;
    [SerializeField] private Text txt_Description;

    private Action yesCallback;
    private Action noCallback;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        this.transform.SetAsLastSibling();

        btn_Yes.onClick.AddListener(() => 
        {
            OnButtonClick();

            if (yesCallback != null)
            {
                yesCallback();
            }

            Exit();
        });

        btn_No.onClick.AddListener(()=> 
        {
            OnButtonClick();

            if (noCallback != null)
            {
                noCallback();
            }

            Exit();
        });

    }


    public void UIUpdate(string _desc, 
        Action _yesCallback = null, 
        Action _noCallback = null)
    {
        this.transform.SetAsLastSibling();

        txt_Description.text = _desc;
        yesCallback = _yesCallback;
        noCallback = _noCallback;

        SlideIn();
    }

    public override void UIUpdate()
    {
    }

    public override void EscapePressed()
    {
        Exit();
    }

    public override void Exit()
    {
        if (isTweening) return;

        SlideOut(() =>
        {
            tr_Body.localPosition = Vector2.zero;
            this.transform.gameObject.SetActive(false);
        });
    }
}
