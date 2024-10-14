using System;
using Enums;

/// <summary>
/// 인게임 내에서 실시간으로 변화하는 플레이어 데이터를 delegate event로 만들어 필요 함수를 추가하여 사용
/// </summary>
public static class PlayerEvent
{
    public delegate void StatusEvent(float value);
    public static StatusEvent healthEvent;
    public static StatusEvent manaEvent;
    public static StatusEvent staminaEvent;
    public static StatusEvent skillPointEvent;
    public static StatusEvent levelUpEvent;

    public delegate void ExpEvent(float value, float maxValue);
    public static ExpEvent expEvent;

    // (+)추가된 스텟 데이터 [장비 착용, 아이템 사용]
    public delegate void AdditionalStatEvent(AdditionalStatType type, float value);
    public static AdditionalStatEvent additionalStatEvent;

    // 스킬 사용 이벤트
    public delegate bool SkillEvent(SkillReferenceData skill, Action callback);
    public static SkillEvent skillEvent;

    // 레벨업 시 변경될 내용들 Callback Event
    public delegate void LevelUpCallbackEvent();
    public static LevelUpCallbackEvent levelupCallbackEvent;
}


