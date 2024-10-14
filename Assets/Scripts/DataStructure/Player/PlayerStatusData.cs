using Enums;
using UnityEngine;

/// <summary>
/// 플레이어 스텟 저장 데이터
/// </summary>
public class PlayerStatusData
{
    public PlayerStatusData(Configurable _config)
    {
        maxData = (MaxStatDataPerLevel)_config;
        
        Initialize();
    }

    MaxStatDataPerLevel maxData;

    // 최종 스탯 데이터 
    public float SP => maxData.SP(level) + additionalStat_SP; // 레벨별 마력 + 추가된 마력
    public float DP => maxData.DP(level) + additionalStat_DP; // 레벨별 방어력 + 추가된 방어력
    public float MaxHP => maxData.MaxHP(level) + additionalStat_MaxHp; // 최대 체력 + 추가된 최대 체력
    public float MaxMP => maxData.MaxMP(level) + additionalStat_MaxMp; // 최대 마력 + 추가된 최대 마력
    public int MaxEXP => maxData.MaxEXP(level); // 레벨별 필요 경험치


    // (+)추가된 스텟 데이터 [장비 착용, 아이템 사용]
    private float additionalStat_SP;
    public float AdditionalStat_SP => additionalStat_SP;

    private float additionalStat_DP;
    public float AdditionalStat_DP => additionalStat_DP;

    private float additionalStat_MaxHp;
    public float AdditionalStat_MaxHp => additionalStat_MaxHp;

    private float additionalStat_MaxMp;
    public float AdditionalStat_MaxMp => additionalStat_MaxMp;


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

    private void Initialize()
    {
        level = PLoad.Load("Level", 1);
        health = PLoad.Load("Health", MaxHP);
        mana = PLoad.Load("Mana", MaxMP);
        exp = PLoad.Load("Exp", 0);
        stamina = 100;
        skillPoint = PLoad.Load("SkillPoint", 1);
    }

    private void LevelUP()
    {
        if (level == 20) return; // 최대 제한 레벨

        SoundManager.sInst.Play("LevelUp");

        Level = +1; HP = MaxHP; MP = MaxMP; Stamina = 100; SkillPoint = +1;

        if (PlayerEvent.levelupCallbackEvent != null) PlayerEvent.levelupCallbackEvent();
        if (EventManager.uiUpdateEvent != null) EventManager.uiUpdateEvent();
    }

    // (+)추가된 스텟 데이터 [장비 착용, 아이템 사용]
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

    // 회복이 가능한 스탯 [아이템 사용]
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
