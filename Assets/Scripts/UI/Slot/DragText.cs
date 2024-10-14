using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragText : MonoBehaviour
{
    [SerializeField] private Text txt;
    public void OnPointerDown(string _txt, Vector2 pos)
    {
        txt.text = _txt;
        this.transform.position = pos;
        this.gameObject.SetActive(true);
    }

    public void OnDrag(Vector2 pos)
    {
        this.transform.position = pos;
    }

    public void OnPointerUp()
    {
        this.gameObject.SetActive(false);
    }
}
