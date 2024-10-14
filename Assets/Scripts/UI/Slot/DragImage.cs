using UnityEngine;
using UnityEngine.UI;

public class DragImage : MonoBehaviour
{
    [SerializeField] private Image img;
    public void OnPointerDown(Sprite sprt, Vector2 pos)
    {
        img.sprite = sprt;
        img.transform.position = pos;
        this.gameObject.SetActive(true);
    }

    public void OnDrag(Vector2 pos)
    {
        img.transform.position = pos;
    }

    public void OnPointerUp()
    {
        this.gameObject.SetActive(false);
    }
}
