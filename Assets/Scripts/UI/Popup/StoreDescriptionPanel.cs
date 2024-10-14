using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class StoreDescriptionPanel : MonoBehaviour
{
    [SerializeField] private Image img_item;
    [SerializeField] private Text txt_Name;
    [SerializeField] private Text txt_Option;
    [SerializeField] private Text txt_Desc;

    public void On(ItemData data, Vector3 pos)
    {
        img_item.sprite = data.Sprite;
        txt_Name.text = data.ItemName;
        txt_Option.text = data.OptionDesc;
        txt_Desc.text = data.Description;

        this.transform.position = pos;

        this.gameObject.SetActive(true);
    }

    public void Off()
    {
        this.gameObject.SetActive(false);
    }
}
