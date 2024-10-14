using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// HP, MP, EXP, Stamina 게이지 UI Handler
/// </summary>
public class PlayerStatusGaugeManager : MonoBehaviour
{
    [SerializeField] private Text txt_Level;

    [SerializeField] private Gauge healthGauge; // HP 게이지
    [SerializeField] private Gauge manaGauge; // MP 게이지
    [SerializeField] private Gauge expGauge; // EXP 게이지
    [SerializeField] private StaminaGauge StaminaGauge; // Stamina 게이지
    
    PlayerStatusData statData; // 저장된 플레이어 스테이터스

    public void InitializePlayerStatusGauge(PlayerController _player)
    {
        statData = DataContainer.sInst.PlayerStatus(); // 저장된 플레이어 스테이터스 로드

        txt_Level.text = $"Lv. {statData.Level}";

        // 게이지들 초기화
        healthGauge.InitializeGauge(statData.HP, statData.MaxHP);
        manaGauge.InitializeGauge(statData.MP, statData.MaxMP);
        expGauge.InitializeGauge(statData.EXP, statData.MaxEXP);
        StaminaGauge.InitializeGauge(statData.Stamina, _player);

        // 이벤트 등록
        PlayerEvent.healthEvent += healthGauge.OnGaugeValueChanged;
        PlayerEvent.manaEvent += manaGauge.OnGaugeValueChanged;
        PlayerEvent.expEvent += expGauge.OnGaugeValueChanged;
        PlayerEvent.staminaEvent += StaminaGauge.OnGaugeValueChanged;
        PlayerEvent.levelUpEvent += OnLevelValueChanged;
    }

    private void OnDisable()
    {
        // 이벤트 해제
        PlayerEvent.healthEvent -= healthGauge.OnGaugeValueChanged;
        PlayerEvent.manaEvent -= manaGauge.OnGaugeValueChanged;
        PlayerEvent.expEvent -= expGauge.OnGaugeValueChanged;
        PlayerEvent.staminaEvent -= StaminaGauge.OnGaugeValueChanged;
        PlayerEvent.levelUpEvent -= OnLevelValueChanged;
    }

    private void OnLevelValueChanged(float value)
    {
        // 레벨업
        txt_Level.text = $"Lv. {value}";
        healthGauge.OnGaugeValueChanged(statData.MaxHP);
        manaGauge.OnGaugeValueChanged(statData.MaxMP);
        expGauge.OnGaugeValueChanged(statData.MaxEXP);
        StaminaGauge.OnGaugeValueChanged(statData.Stamina);
    }
}
