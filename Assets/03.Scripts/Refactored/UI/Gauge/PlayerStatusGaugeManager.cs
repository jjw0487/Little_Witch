using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusGaugeManager : MonoBehaviour
{
    [SerializeField] private Text txt_Level;

    [SerializeField] private Gauge healthGauge;
    [SerializeField] private Gauge manaGauge;
    [SerializeField] private Gauge expGauge;
    [SerializeField] private StaminaGauge StaminaGauge;
    
    PlayerStatusData statData;

    public void InitializePlayerStatusGauge(PlayerController _player)
    {
        statData = DataContainer.sInst.PlayerStatus();

        txt_Level.text = $"Lv. {statData.Level}";

        healthGauge.InitializeGauge(statData.HP, statData.MaxHP);
        manaGauge.InitializeGauge(statData.MP, statData.MaxMP);
        expGauge.InitializeGauge(statData.EXP, statData.MaxEXP);
        StaminaGauge.InitializeGauge(statData.Stamina, _player);

        PlayerEvent.healthEvent += healthGauge.OnGaugeValueChanged;
        PlayerEvent.manaEvent += manaGauge.OnGaugeValueChanged;
        PlayerEvent.expEvent += expGauge.OnGaugeValueChanged;
        PlayerEvent.staminaEvent += StaminaGauge.OnGaugeValueChanged;
        PlayerEvent.levelUpEvent += OnLevelValueChanged;
    }

    private void OnDisable()
    {
        PlayerEvent.healthEvent -= healthGauge.OnGaugeValueChanged;
        PlayerEvent.manaEvent -= manaGauge.OnGaugeValueChanged;
        PlayerEvent.expEvent -= expGauge.OnGaugeValueChanged;
        PlayerEvent.staminaEvent -= StaminaGauge.OnGaugeValueChanged;
        PlayerEvent.levelUpEvent -= OnLevelValueChanged;
    }

    private void OnLevelValueChanged(float value)
    {
        txt_Level.text = $"Lv. {value}";
        healthGauge.OnGaugeValueChanged(statData.MaxHP);
        manaGauge.OnGaugeValueChanged(statData.MaxMP);
        expGauge.OnGaugeValueChanged(statData.MaxEXP);
        StaminaGauge.OnGaugeValueChanged(statData.Stamina);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F3))
        {
            statData.HP = +10;
            statData.MP = +10;
            statData.EXP = +50;
            statData.Stamina = +10;
        }
    }

}
