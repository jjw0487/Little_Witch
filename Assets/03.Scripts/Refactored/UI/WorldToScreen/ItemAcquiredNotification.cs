using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ItemAcquiredNotification : MonoBehaviour
{
    [SerializeField] private Image img_Item;
    [SerializeField] private Text txt_ItemName;
    [SerializeField] private Text txt_Desc;

    private bool isOn = false;

    public bool IsOn() => isOn;

    public void Spawn(ItemData item)
    {
        isOn = true;

        img_Item.sprite = item.Sprite;

        txt_ItemName.text = $"[{item.ItemName}] Item Acquired!";

        txt_Desc.text = item.Description;

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
