using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FloatingDamage : MonoBehaviour
{
    [SerializeField] private Text txt;

    private bool isOn = false;

    public bool IsOn() => isOn;

    public void Spawn(Vector3 _pos, float _value)
    {
        isOn = true;

        this.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, _pos); ;

        txt.color = Color.black;

        int dmg = (int)_value;

        txt.text = dmg.ToString();

        this.gameObject.SetActive(true);

        txt.transform.DOShakePosition(0.3f).OnComplete(() => 
        {
            txt.DOFade(0.1f, 0.5f).OnComplete(() => { Despawn(); });
        });
    }
    public void Despawn()
    {
        isOn = false;

        this.gameObject.SetActive(false);
    }    
}
