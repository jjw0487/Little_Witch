
using UnityEngine;

[CreateAssetMenu(fileName = "DebuffSkillData", menuName = "ScriptableObject/DebuffSkillData", order = 4)]
public class DebuffSkillData : SkillData
{
    [Header("Selective::Debuff")]
    [SerializeField] private float[] debuffDuration;

    [SerializeField] private float[] debuffPercentage;

    public override float DebuffDuration(int _level) => debuffDuration[_level];
    public override float DebuffPercentage(int _level) => debuffPercentage[_level];
}
