using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System;

[Serializable]
public class SkillUnit
{
    public SkillKey identity;
    public string playerPrefs;
    public SkillData data;
}

/// <summary>
/// ���� �÷��� �� ����� �����͸� ������� ���� ���� �� SkillReferenceData���� �����ϸ�
/// ��ų �����͸� ���ϴ� ���� ����
/// </summary>
public class SkillManager : MonoBehaviour
{
    [SerializeField] private List<SkillUnit> list;

    private Dictionary<SkillKey, SkillReferenceData> skills
        = new Dictionary<SkillKey, SkillReferenceData>();

    private PlayerStatusData statData;
    public int SkillPoint => statData.SkillPoint;

    public void InitializeSkillManager(PlayerStatusData playerStatData)
    {
        statData = playerStatData;

        InitializeSkillReference();
    }

    public void DecreaseSkillPoint()
    {
        statData.SkillPoint = -1;

        // Skill point �� ǥ���ϴ� UI�� ����Ǿ� �ִٸ� ��Ȯ�� ��ġ�� ������Ʈ ���ش�.
        if (EventManager.uiUpdateEvent != null) EventManager.uiUpdateEvent(); 
    }

    private void InitializeSkillReference()
    {
        GameObject skillPool = new GameObject("SkillObjectPool");

        for(int i = 0; i < list.Count; i++) 
        {
            SkillUnit unit = list[i];

            SkillReferenceData refData = 
                new SkillReferenceData(unit.playerPrefs, unit.data, skillPool.transform);

            skills.Add(unit.identity, refData);
        }
    }

    public SkillReferenceData GetSkillData(SkillKey key)
    {
        if (skills.TryGetValue(key, out SkillReferenceData value))
        {
            return value;
        }

        return null;
    }
}

