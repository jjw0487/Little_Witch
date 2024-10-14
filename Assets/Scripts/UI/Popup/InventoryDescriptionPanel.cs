using UnityEngine;
using UnityEngine.UI;

public class InventoryDescriptionPanel : MonoBehaviour
{
    [SerializeField] private Image img_item;
    [SerializeField] private Text txt_Name;
    [SerializeField] private Text txt_Option;
    [SerializeField] private Text txt_Desc;

    public void On(Sprite sprt, string name, string option, string desc, Vector3 pos)
    {
        img_item.sprite = sprt;
        txt_Name.text = name;
        txt_Option.text = option;
        txt_Desc.text = desc;

        this.transform.position = pos;

        this.gameObject.SetActive(true);
    }

    public void Off()
    {
        this.gameObject.SetActive(false);
    }


}
