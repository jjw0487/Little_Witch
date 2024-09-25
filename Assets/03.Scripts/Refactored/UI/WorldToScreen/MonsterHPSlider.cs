using UnityEngine;
using UnityEngine.UI;

public class MonsterHPSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;

    [SerializeField] private Animator exMark;

    private bool isOn = false;
    public bool IsOn() => isOn;

    public void Spawn(float _curValue, float _maxValue, float localScale)
    {
        isOn = true;

        slider.minValue = 0;
        slider.maxValue = _maxValue;

        slider.value = _curValue;

        this.transform.localScale = new Vector3(localScale, localScale, localScale);

        this.gameObject.SetActive(true);

        exMark.SetTrigger("Exclamation");
    }
    public void Despawn()
    {
        isOn = false;

        this.gameObject.SetActive(false);
    }

    public void FollowTarget(Vector3 _pos) 
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, _pos);
        this.transform.position = screenPoint;
    }

    public void GetHit(float value)
    {
        slider.value = value;
    }

    

}
