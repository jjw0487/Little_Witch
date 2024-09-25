using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Gauge : MonoBehaviour
{
    [SerializeField] protected Slider gauge;
    [SerializeField] protected Text txt_CurValue;

    public virtual void InitializeGauge(float curVal, float maxVal)
    {
        gauge.wholeNumbers = true;
        gauge.maxValue = maxVal;
        gauge.value = curVal;

        txt_CurValue.text = curVal.ToString();
    }

    public abstract void OnGaugeValueChanged(float value);
    public virtual void OnGaugeValueChanged(float value, float maxValue) { }
}
