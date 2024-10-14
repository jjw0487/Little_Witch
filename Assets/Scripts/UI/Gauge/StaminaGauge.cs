
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 플레이어를 따라다니는 스테미너 게이지 바
/// </summary>
public class StaminaGauge : MonoBehaviour
{
    [SerializeField] private Image img_Fill; // 바깥에 보이는 게이지 바
    [SerializeField] private Image img_BGFill; // 안쪽에 소모된 게이지를 보여주는 바
    [SerializeField] private Text txt_Value; // 수치

    private PlayerController player; 
    private int curStaminaValue;
    private bool isOn;

    private void FixedUpdate()
    {
        if(isOn)
        {
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, 
                player.StaminaGaugePosition().position);

            this.transform.position = screenPoint;
        }
    }

    public void InitializeGauge(float value, PlayerController _player)
    {
        curStaminaValue = (int)value;

        player = _player;

        isOn = false;

        img_Fill.fillAmount = curStaminaValue * 0.01f;
        img_BGFill.fillAmount = curStaminaValue * 0.01f;
        txt_Value.text = curStaminaValue.ToString();

        if (curStaminaValue < 100) Appear();
        else Disappear();
    }
    private void Appear() // 노출
    {
        if (isOn) return;

        isOn = true;

        this.transform.DOScale(1f, 0.2f).OnComplete(() =>
        {
            if (!isOn) Disappear();
        });
    }

    private void Disappear() // 모두 회복되었을 때 사라짐
    {
        if (!isOn) return;

        isOn = false;

        this.transform.DOScale(0f, 0.2f).OnComplete(() =>
        {
            if (isOn) Appear();
        });
    }



    public void OnGaugeValueChanged(float value)
    {
        curStaminaValue = (int)value;

        img_Fill.fillAmount = curStaminaValue * 0.01f;

        txt_Value.text = curStaminaValue.ToString();

        if(curStaminaValue == 100)
        {
            img_BGFill.fillAmount = 1f;
        }

        if (curStaminaValue < 100) Appear();
        else Disappear();
    }

    public void OnBGGaugeValueChanged()
    {
        float bg = img_BGFill.fillAmount;
        float main = img_Fill.fillAmount;

        if (Mathf.Approximately(bg, main)) return;
        else if(bg > main) img_BGFill.fillAmount -= 0.01f;
        else if(bg < main) img_BGFill.fillAmount += 0.01f;
    }
    
}
