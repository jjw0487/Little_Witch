using Enums;
using UnityEngine;
public class PlayerStatusData
{
    public PlayerStatusData(Configurable _config)
    {
        maxData = (MaxStatDataPerLevel)_config;
        
        Initialize();
    }

    MaxStatDataPerLevel maxData;

    private int level;
    public int Level
    {
        get => level;
        set
        {
            if (level + value > 20) return;

            level = Mathf.Clamp(level + value, 1, 20);

            PlayerEvent.levelUpEvent(level);

            PSave.Save("Level", level);
        }
    }

    private float health;
    public float HP
    {
        get => health;
        set {

            health = Mathf.Clamp(health + value, 0, MaxHP);

            PlayerEvent.healthEvent(health);

            PSave.Save("Health", health);
        }
    }

    private float mana;
    public float MP
    {
        get => mana;
        set
        {
            mana = Mathf.Clamp(mana + value, 0, MaxMP);

            PlayerEvent.manaEvent(mana);

            PSave.Save("Mana", mana);
        }
    }

    private float stamina;
    public float Stamina
    {
        get => stamina;
        set
        {
            stamina = Mathf.Clamp(stamina + value, 0, 100);

            PlayerEvent.staminaEvent(stamina);

            // 스태미너는 기록하지 말자..
            //PSave.Save("Stamina", stamina);
        }
    }

    private int skillPoint;
    public int SkillPoint
    {
        get => skillPoint;
        set
        {
            skillPoint = Mathf.Clamp(skillPoint + value, 0, 21);
            PSave.Save("SkillPoint", skillPoint);
        }
    }


    private int exp;
    public int EXP
    {
        get => exp;
        set
        {
            int compare = exp + value;

            if (compare >= MaxEXP)
            {
                exp = compare - MaxEXP;
                LevelUP();
            }
            else
            {
                exp = Mathf.Clamp(compare, 0, MaxEXP);
            }

            PlayerEvent.expEvent(exp, MaxEXP);

            PSave.Save("Exp", exp);
        }
    }

    public float SP => maxData.SP(level) + additionalStat_SP;
    public float DP => maxData.DP(level) + additionalStat_DP;
    public float MaxHP => maxData.MaxHP(level) + additionalStat_MaxHp;
    public float MaxMP => maxData.MaxMP(level) + additionalStat_MaxMp;
    public int MaxEXP => maxData.MaxEXP(level);

    private float additionalStat_SP;
    public float AdditionalStat_SP => additionalStat_SP;

    private float additionalStat_DP;
    public float AdditionalStat_DP => additionalStat_DP;

    private float additionalStat_MaxHp;
    public float AdditionalStat_MaxHp => additionalStat_MaxHp;

    private float additionalStat_MaxMp;
    public float AdditionalStat_MaxMp => additionalStat_MaxMp;

    private void Initialize()
    {
        level = PLoad.Load("Level", 1);
        health = PLoad.Load("Health", MaxHP);
        mana = PLoad.Load("Mana", MaxMP);
        exp = PLoad.Load("Exp", 0);
        stamina = 100; // 스태미너는 기록하지 말자..
        skillPoint = PLoad.Load("SkillPoint", 1);
    }

    private void LevelUP()
    {
        if (level == 20) return;

        SoundManager.sInst.Play("LevelUp");

        Level = +1;
        HP = MaxHP;
        MP = MaxMP;
        Stamina = 100;
        SkillPoint = +1;

        if (PlayerEvent.levelupCallbackEvent != null) PlayerEvent.levelupCallbackEvent();
        if (EventManager.uiUpdateEvent != null) EventManager.uiUpdateEvent();
    }

    public void ChangeAdditionalStatValue(AdditionalStatType type, float value)
    {
        switch (type)
        {
            case AdditionalStatType.SP: additionalStat_SP += value; break;
            case AdditionalStatType.DP: additionalStat_DP += value; break;
            case AdditionalStatType.MaxHP: additionalStat_MaxHp += value; break;
            case AdditionalStatType.MaxMp: additionalStat_MaxMp += value; break;
            default: break;
        }
    }

    public void RecoveryStatValue(ConsumableItemType type, float value)
    {
        switch (type)
        {
            case ConsumableItemType.HP: HP = +value; break;
            case ConsumableItemType.MP: MP = +value; break;
            case ConsumableItemType.ST: Stamina = +value; break;
            default: break;
        }
    }
}
