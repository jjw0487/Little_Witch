public class ManaGauge : Gauge
{
    public override void InitializeGauge(float curVal, float maxVal)
    {
        base.InitializeGauge(curVal, maxVal);
    }

    public override void OnGaugeValueChanged(float value)
    {
        if (value > gauge.maxValue)
        { // ·¹º§¾÷
            gauge.maxValue = value;
            gauge.value = value;

            return;
        }
        else
        {
            gauge.value = value;

        }

        txt_CurValue.text = $"{(int)gauge.value}";
    }

}
