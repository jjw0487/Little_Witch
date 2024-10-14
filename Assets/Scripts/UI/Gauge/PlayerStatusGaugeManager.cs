using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// HP, MP, EXP, Stamina ������ UI Handler
/// </summary>
public class PlayerStatusGaugeManager : MonoBehaviour
{
    [SerializeField] private Text txt_Level;

    [SerializeField] private Gauge healthGauge; // HP ������
    [SerializeField] private Gauge manaGauge; // MP ������
    [SerializeField] private Gauge expGauge; // EXP ������
    [SerializeField] private StaminaGauge StaminaGauge; // Stamina ������
    
    PlayerStatusData statData; // ����� �÷��̾� �������ͽ�

    public void InitializePlayerStatusGauge(PlayerController _player)
    {
        statData = DataContainer.sInst.PlayerStatus(); // ����� �÷��̾� �������ͽ� �ε�

        txt_Level.text = $"Lv. {statData.Level}";

        // �������� �ʱ�ȭ
        healthGauge.InitializeGauge(statData.HP, statData.MaxHP);
        manaGauge.InitializeGauge(statData.MP, statData.MaxMP);
        expGauge.InitializeGauge(statData.EXP, statData.MaxEXP);
        StaminaGauge.InitializeGauge(statData.Stamina, _player);

        // �̺�Ʈ ���
        PlayerEvent.healthEvent += healthGauge.OnGaugeValueChanged;
        PlayerEvent.manaEvent += manaGauge.OnGaugeValueChanged;
        PlayerEvent.expEvent += expGauge.OnGaugeValueChanged;
        PlayerEvent.staminaEvent += StaminaGauge.OnGaugeValueChanged;
        PlayerEvent.levelUpEvent += OnLevelValueChanged;
    }

    private void OnDisable()
    {
        // �̺�Ʈ ����
        PlayerEvent.healthEvent -= healthGauge.OnGaugeValueChanged;
        PlayerEvent.manaEvent -= manaGauge.OnGaugeValueChanged;
        PlayerEvent.expEvent -= expGauge.OnGaugeValueChanged;
        PlayerEvent.staminaEvent -= StaminaGauge.OnGaugeValueChanged;
        PlayerEvent.levelUpEvent -= OnLevelValueChanged;
    }

    private void OnLevelValueChanged(float value)
    {
        // ������
        txt_Level.text = $"Lv. {value}";
        healthGauge.OnGaugeValueChanged(statData.MaxHP);
        manaGauge.OnGaugeValueChanged(statData.MaxMP);
        expGauge.OnGaugeValueChanged(statData.MaxEXP);
        StaminaGauge.OnGaugeValueChanged(statData.Stamina);
    }
}
