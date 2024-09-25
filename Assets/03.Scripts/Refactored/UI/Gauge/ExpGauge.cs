using static UnityEngine.Rendering.DebugUI;

public class ExpGauge : Gauge
{
    public override void InitializeGauge(float curVal, float maxVal)
    {
        gauge.wholeNumbers = true;
        gauge.maxValue = maxVal;
        gauge.value = curVal;
        float percentage = maxVal / curVal * 100;

        string form = curVal > 0 ? string.Format("{0:N2}", percentage): "0";

        txt_CurValue.text = $"{curVal}/{maxVal} [{form}%]";
    }

    public override void OnGaugeValueChanged(float value)
    {
    }

    public override void OnGaugeValueChanged(float value, float maxValue)
    {
        gauge.maxValue = maxValue;
        gauge.value = value;

        float percentage = value / maxValue * 100;

        string form = value > 0 ? string.Format("{0:N2}", percentage) : "0";

        txt_CurValue.text = $"{value}/{maxValue} [{form}%]";
    }

}
