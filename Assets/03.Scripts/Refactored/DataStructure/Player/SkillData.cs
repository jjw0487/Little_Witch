using UnityEngine;
using Enums;

public abstract class SkillData : ScriptableObject
{

    [Header("Mutual Data")]

    [SerializeField] private SkillKey key;
    public SkillKey Key => key;

    [SerializeField] private SkillType type;
    public SkillType Type => type;


    [SerializeField] private string skillName;
    public string SkillName => skillName;

    [SerializeField] private string typeName;
    public string TypeName => typeName;

    [SerializeField] private int restrictedLevel;
    public int RestrictedLevel => restrictedLevel;

    [SerializeField] private string description;
    public string Description => description;


    [SerializeField] private bool isWaitBeforeAction;
    public bool IsWaitBeforeAction => isWaitBeforeAction;

    [SerializeField] private bool isImmediate;
    public bool IsImmediate => isImmediate;

    [SerializeField] private float castingDuration; // [시전시간] 애니메이션 동작동안 움직임을 막기 위함
    public float CastingDuration => castingDuration;

    [SerializeField] private Vector3 performOffset;
    public Vector3 PerformOffset => performOffset;

    [SerializeField] private float overlapRadius;
    public float OverlapRadius => overlapRadius;


    [SerializeField] private float skillRange; // 스킬 사용가능 사정거리
    public float SkillRange => skillRange;

    [SerializeField] private string trigger;
    public string Trigger => trigger;

    [SerializeField] private Sprite sprt;
    public Sprite  Sprt => sprt;

    [SerializeField] private SkillObject effect;
    public SkillObject Effect => effect;

    [SerializeField] private int objectPoolAmount;
    public int ObjectPoolAmount => objectPoolAmount;




    [Header("Growth Data")]

    [SerializeField] private float[] damage;
    public float Damage(int _level) => damage[_level];

    [SerializeField] private float[] coolTime;
    public float CoolTime(int _level) => coolTime[_level];

    [SerializeField] private float[] mpConsumption;
    public float MPConsumption(int _level) => mpConsumption[_level];

    public abstract float DebuffPercentage(int _level);
    public abstract float DebuffDuration(int _level);

}
