using System;
using Enums;

public static class PlayerEvent
{
    public delegate void StatusEvent(float value);
    public static StatusEvent healthEvent;
    public static StatusEvent manaEvent;
    public static StatusEvent staminaEvent;
    public static StatusEvent skillPointEvent;
    public static StatusEvent levelUpEvent;

    public delegate void AdditionalStatEvent(AdditionalStatType type, float value);
    public static AdditionalStatEvent additionalStatEvent;

    public delegate void ExpEvent(float value, float maxValue);
    public static ExpEvent expEvent;

    public delegate bool SkillEvent(SkillReferenceData skill, Action callback);
    public static SkillEvent skillEvent;

    public delegate void LevelUpCallbackEvent();
    public static LevelUpCallbackEvent levelupCallbackEvent;
}


