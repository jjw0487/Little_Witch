///////////////////////////////////////////////////////////////////////////
///
/// PopupController.cs
/// 
/// Jeongwoo, Jeon  2024
/// 
///////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using UnityEngine;

public class PopupController
{
    protected Transform parent;
    protected Stack<GameObject> stack;
    protected Dictionary<string, GameObject> popups;

    protected PopupList popupList;

    public PopupController(Transform _parent, PopupList _popupList)
    {
        parent = _parent;
        stack = new Stack<GameObject>();
        popups = new Dictionary<string, GameObject>();

        popupList = _popupList;
    }

    public virtual void ShowPopup(string popup, bool stacking)
    {
        if (popups.TryGetValue(popup, out GameObject value))
        {
            if (!value.gameObject.activeSelf)
            {
                value.GetComponent<IPopup>().UIUpdate();
                value.gameObject.SetActive(true);

                if (stacking)
                {
                    stack.Push(value);
                }
            }
        }
        else
        {
            var obj = popups[popup] = popupList.GetPopup(popup, parent);
            // 중복 생성을 방지 위해 딕셔너리에 저장

            //obj.GetComponent<IPopup>().UIUpdate();

            if (stacking)
            {
                stack.Push(obj);
            }
        }
    }

    public virtual void ClosePopup(string popup)
    {
        if (popups.TryGetValue(popup, out GameObject value))
        {
            value.gameObject.SetActive(false);
        }
    }

    public virtual GameObject GetDisposablePopup(string popup)
    {
        var obj = popupList.GetPopup(popup, parent);
        return obj;
    }

    public virtual GameObject ShowAndGetPopup(string popup, bool stacking)
    {
        if (popups.TryGetValue(popup, out GameObject value))
        {
            if (!value.gameObject.activeSelf)
            {
                value.gameObject.SetActive(true);
                value.GetComponent<IPopup>().UIUpdate();

                if (stacking)
                {
                    stack.Push(value);
                }
            }

            return value;
        }
        else
        {
            var obj = popups[popup] =
                popupList.GetPopup(popup, parent);
            // 중복 생성을 방지 위해 딕셔너리에 저장
            //obj.GetComponent<IPopup>().UIUpdate();

            if (stacking)
            {
                stack.Push(obj);
            }

            return obj;
        }
    }

    public virtual void EscapePressed()
    {
        if (stack.Count > 0)
        {
            for (int i = 0; i < stack.Count; i++)
            {
                if (stack.Peek().activeSelf)
                {
                    stack.Peek().GetComponent<IPopup>().EscapePressed();

                    return;
                }
                else
                {
                    stack.Pop();
                }
            }

            stack.Clear();
        }
        else
        {
            UIManager.inst.ShowPopup("Setting", true);
        }
    }


}
