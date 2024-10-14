using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 재사용 대기시간을 UI로 표현함
/// </summary>
public class CoolTimer : MonoBehaviour
{
    [SerializeField] private Text txt_CoolTime; // 시간
    [SerializeField] private Image img_Fill; // 슬라이더 이미지

    private bool isCoolTime = false;
    public bool IsCoolTime => isCoolTime;

    private float coolTime;
    private float count;

    public void Init(float _coolTime)
    {
        coolTime = _coolTime;

        count = _coolTime;

        txt_CoolTime.text = GetParse(coolTime);

        img_Fill.fillAmount = 1f;

        this.gameObject.SetActive(true);

        isCoolTime = true;
    }
    private void FixedUpdate()
    {
        if (isCoolTime)
        {
            CountDown();
        }
    }
    private void CountDown()
    {
        if (count > 0.02)
        {
            count -= Time.deltaTime;
            txt_CoolTime.text = GetParse(count);

            img_Fill.fillAmount = count / coolTime;
        }
        else
        {
            isCoolTime = false;

            this.gameObject.SetActive(false);
        }
    }

    
    private string GetParse(float remaining)
    {
        return GetSecond(remaining) + GetMillisec(remaining);
    }


    private string GetSecond(float t)
    {
        string secondsFormatted;
        int sec = (int)t % 60;
        secondsFormatted = string.Format("{0:00}", sec);
        secondsFormatted += ":";
        return secondsFormatted;
    }

    private string GetMillisec(float t)
    {
        string millis = string.Format("{0:.00}", t % 1);
        millis = millis.Replace(".", "");

        if (millis.Equals("100")) return "00";

        return millis;
    }

}
