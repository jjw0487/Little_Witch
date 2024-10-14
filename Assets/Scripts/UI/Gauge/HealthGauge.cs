using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthGauge : Gauge
{
    [SerializeField] private Slider bgGauge;

    private Coroutine co;

    private float target;
    public override void InitializeGauge(float curVal, float maxVal)
    {
        base.InitializeGauge(curVal, maxVal);

        bgGauge.wholeNumbers = true;
        bgGauge.maxValue = maxVal;
        bgGauge.value = curVal;
    }

    public override void OnGaugeValueChanged(float value)
    {
        if (value > gauge.maxValue)
        { // ·¹º§¾÷
            gauge.maxValue = value;
            gauge.value = value;

            bgGauge.maxValue = value;
            bgGauge.value = value;

            return;
        }
        else
        {
            gauge.value = value;

            target = value;

            if (bgGauge.value > value)
            {
                if (co == null)
                {
                    co = StartCoroutine(LerpBGSlider());
                }
            }
            else
            {
                bgGauge.value = value;
            }
        }

        txt_CurValue.text = $"{(int)gauge.value}";
    }

    IEnumerator LerpBGSlider()
    {
        float wait = 0.6f;
        int frame = 10;
        int less = frame;

        while(true)
        {
            wait -= Time.deltaTime;

            if (wait < 0f)
            {
                less--;

                if(less < 0)
                {
                    bgGauge.value -= 1; // cos whole number is true => Round(value); 

                    if (bgGauge.value <= target)
                    {
                        bgGauge.value = target;
                        co = null;
                        yield break;
                    }

                    less = frame;
                }
            }

            yield return null;
        } 

        
    }
}
