using UnityEngine;

public class StatPointRecoveryManager : MonoBehaviour
{
    [SerializeField] private float staminaRecoveryValuePerSec = 10f;
    [SerializeField] private float healthRecoveryValuePerSec = 1f;
    [SerializeField] private float manaRecoveryValuePerSec = 2f;

    [SerializeField] private StaminaGauge staminaGauge;

    PlayerStatusData statData;

    private float sec = 0f;
    private float rMana => manaRecoveryValuePerSec + (statData.Level * manaRecoveryValuePerSec * 0.1f);
    private float rHealth => healthRecoveryValuePerSec + (statData.Level * healthRecoveryValuePerSec * 0.1f);
    private float rStamina => staminaRecoveryValuePerSec;

    private float curStamina = 0f;
    private int breathTimer = 0;


    private bool isHealthMax;
    private bool isManaMax;
    private bool isStaminaMax;

    private bool isPlayerDied = false;

    

    private void Start()
    {
        InitializeRecoveryManager();
    }

    private void OnDisable()
    {
        PlayerEvent.healthEvent -= OnHealthValueChanged;
        PlayerEvent.manaEvent -= OnManaValueChanged;
        PlayerEvent.staminaEvent -= OnStaminiValueChanged;
    }

    private void InitializeRecoveryManager()
    {
        statData = DataContainer.sInst.PlayerStatus();

        sec = Time.time;
        PlayerEvent.healthEvent += OnHealthValueChanged;
        PlayerEvent.manaEvent += OnManaValueChanged;
        PlayerEvent.staminaEvent += OnStaminiValueChanged;
    }

    private void OnStaminiValueChanged(float value)
    {
        if (curStamina > value) breathTimer = 0;
 
        curStamina = value;
        isStaminaMax = value == 100;
    }
    private void OnHealthValueChanged(float value)
    {
        if (value == 0) isPlayerDied = true;

        isHealthMax = value == statData.MaxHP;
    }
    private void OnManaValueChanged(float value)
    {
        isManaMax = value == statData.MaxMP;
    }

    private void FixedUpdate()
    {
        if(!isPlayerDied)
        {
            if (Time.time - sec > 1f)
            {
                sec = Time.time;

                if (!isHealthMax) statData.HP = +rHealth;

                if (!isManaMax) statData.MP = +rMana;

                breathTimer++;
            }

            if(breathTimer > 1)
            {
                if(!isStaminaMax)
                {
                    statData.Stamina = + rStamina * Time.deltaTime;
                    staminaGauge.OnBGGaugeValueChanged();
                }
            }
        }
    }
}
