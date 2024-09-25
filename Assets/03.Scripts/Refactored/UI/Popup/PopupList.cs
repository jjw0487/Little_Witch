using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GamePopup
{
    public string key;
    public GameObject value;
}

public class PopupList
{
    public PopupList(List<GamePopup> _popUp)
    {
        popupList = _popUp;
    }

    private List<GamePopup> popupList;
    public GameObject GetPopup(string key, Transform t)
    {
        for (int i = 0; i < popupList.Count; i++)
        {
            if (key == popupList[i].key)
            {
                return GameObject.Instantiate(popupList[i].value, t);
            }
        }

        return null;
    }
}