using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPlayerStatusPanel : MonoBehaviour
{
    [SerializeField] private InventoryPlayerStatusPanelUnit stat_Level;
    [SerializeField] private InventoryPlayerStatusPanelUnit stat_Exp;
    [SerializeField] private InventoryPlayerStatusPanelUnit stat_SP;
    [SerializeField] private InventoryPlayerStatusPanelUnit stat_DP;
    [SerializeField] private InventoryPlayerStatusPanelUnit stat_MaxHp;
    [SerializeField] private InventoryPlayerStatusPanelUnit stat_MaxMp;

    private PlayerStatusData statData;

    private void OnEnable()
    {
        EventManager.uiUpdateEvent += UIUpdate;
    }
    private void OnDisable()
    {
        EventManager.uiUpdateEvent -= UIUpdate;
    }

    public void Initialize(PlayerStatusData _statData)
    {
        statData = _statData;

        UIUpdate();
    }

    public void UIUpdate()
    {

        float curVal = statData.EXP;
        float maxVal = statData.MaxEXP;
        float percentage = maxVal / curVal * 100;
        string form = curVal > 0 ? string.Format("{0:N2}", percentage) : "0";

        stat_Level.UIUpdate(statData.Level.ToString());
        stat_Exp.UIUpdate($"{curVal}/{maxVal} [{form}%]");
        stat_SP.UIUpdate(statData.SP.ToString(), $"(+{statData.AdditionalStat_SP})");
        stat_DP.UIUpdate(statData.DP.ToString(), $"(+{statData.AdditionalStat_DP})");
        stat_MaxHp.UIUpdate(statData.MaxHP.ToString(), $"(+{statData.AdditionalStat_MaxHp})");
        stat_MaxMp.UIUpdate(statData.MaxMP.ToString(), $"(+{statData.AdditionalStat_MaxMp})");
    }
}
