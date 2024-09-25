using UnityEngine;
using Enums;

/// <summary>
/// ��ũ���ͺ� ������Ʈ���� ������ �´� �����͸� �����ϰ�
/// �̺�Ʈ�� ���� ��� ��� �����͸� �����ϴ� ����
/// </summary>
public class SkillReferenceData
{
    /// <summary>
    /// Ư�� ��ų���� �ʿ���, ������ ��ų���� �ʿ����� ���� �����ʹ� ��� ó������ �����ؾ��Ѵ�.
    /// </summary>
    public SkillReferenceData(string _playerPrefs, SkillData _data, Transform _objectPoolParent)
    {
        playerPrefs = _playerPrefs;

        level = PLoad.Load(playerPrefs, 0);

        if(level == 0 && _data.Type == SkillType.NormalAttack)
        { // �⺻������ ���۰� ���ÿ� 1
            level = 1;
        }

        data = _data;

        pool = new(data.Effect, data.ObjectPoolAmount, _objectPoolParent, this);
    }

    private int level;
    private string playerPrefs;
    private SkillData data;
    public SkillData Data => data;
    private SkillObjectPooler pool;
    
    public bool LevelUp()
    {
        if (level > 4) return false;

        level++;

        if(level > 5) level = 5;

        PSave.Save(playerPrefs, level);

        return true;
    }

    public void SkillEffect(Vector3 strat, Vector3 target, 
        Transform player, float playerSP)
    {
        pool.GetObj().Spawn(strat, target, player, playerSP);
    }

    public SkillKey Key => data.Key;
    public SkillType Type => data.Type;
    public bool IsWaitBeforeAction => data.IsWaitBeforeAction;
    public bool IsImmediate => data.IsImmediate;
    public float CastingDuration => data.CastingDuration;
    public Vector3 PerformOffset => data.PerformOffset;
    public float OverlapRadius => data.OverlapRadius;
    public float SkillRange => data.SkillRange;
    public string Trigger => data.Trigger;
    public Sprite Sprt => data.Sprt;
    public float Damage => data.Damage(level - 1);
    public float CoolTime => data.CoolTime(level - 1);
    public float MPConsumption => data.MPConsumption(level - 1);
    public float DebuffPercentage => data.DebuffPercentage(level - 1);
    public float DebuffDuration => data.DebuffDuration(level - 1);
    public int Level => level;
    public bool IsMaxLevel() => level == 5;
}
