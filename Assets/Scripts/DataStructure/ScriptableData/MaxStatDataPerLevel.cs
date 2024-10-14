using UnityEngine;

/// <summary>
/// 플레이어의 레벨별 성장 스탯을 관리
/// </summary>
[CreateAssetMenu(fileName = "MaxStatDataPerLevel", menuName = "ScriptableObject/MaxStatDataPerLevel", order = 2)]
public class MaxStatDataPerLevel : Configurable
{
    [SerializeField]
    private float[] hp;
    public float MaxHP(int level) => hp[level - 1];

    [SerializeField]
    private float[] mp;
    public float MaxMP(int level) => mp[level - 1];

    [SerializeField]
    private float[] sp;
    public float SP(int level) => sp[level - 1];

    [SerializeField]
    private float[] dp;
    public float DP(int level) => dp[level - 1];

    [SerializeField]
    private int[] exp;
    public int MaxEXP(int level) => exp[level - 1];
}
