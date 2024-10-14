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
/// 게임 플레이 중 저장된 데이터를 기반으로 게임 시작 시 SkillReferenceData들을 생성하며
/// 스킬 데이터를 요하는 곳에 전달
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

        // Skill point 를 표시하는 UI가 노출되어 있다면 정확하 수치로 업데이트 해준다.
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

