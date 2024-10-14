using UnityEngine;

[CreateAssetMenu(fileName = "AttackSkillData", menuName = "ScriptableObject/AttackSkillData", order = 1)]

public class AttackSkillData : SkillData
{
    public override float DebuffDuration(int _level) => 0f;
    public override float DebuffPercentage(int _level) => 0f;
}
