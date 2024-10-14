using System;
using UnityEngine;

/// <summary>
/// Monster ScriptableObject
/// </summary>
[CreateAssetMenu(fileName = "MonsterData", menuName = "ScriptableObject/MonsterData", order = 1)]

public class MonsterData : ScriptableObject
{
    [SerializeField] private string monsterName;
    public string Name => monsterName;

    [SerializeField] private float health;
    public float HP => health;

    [SerializeField] private float attackPoint;
    public float ATP => attackPoint;

    [SerializeField] private float defencePoint;
    public float DP => defencePoint;

    [SerializeField] private float strikingDistance;
    public float STKDist => strikingDistance;

    [SerializeField] private float agentSpeed;
    public float Speed => agentSpeed;

    [SerializeField] private float agentStopOffsetDistance;
    public float StopDist => agentStopOffsetDistance;

    [SerializeField] private float attackRadius;
    public float AttackRadius => attackRadius;

    [SerializeField] private float attackSpeed;
    public float AttackSpeed => attackSpeed;

    [SerializeField] private int exp;
    public int EXP => exp;

    [SerializeField] private float hpScale;
    public float HPScale => hpScale;

    [SerializeField] private int dropGold;

    [SerializeField] private DropItem[] dropItem;
    
    public void DropItem(Vector3 pos) // 몬스터 사망 시 드랍될 아이템 결정
    {
        for(int i = 0; i < dropItem.Length; i++)
        {
            int randNum = UnityEngine.Random.Range(0, 101);

            Debug.Log("randNum : " + randNum);

            if (dropItem[i].probability < randNum) continue;

            if (EventManager.itemSpawnEvent != null)
            {
                EventManager.itemSpawnEvent(pos, 1, dropItem[i].item);
            }
        }
    }

    public void GoldAndExpEvent() //  몬스터 사망 시 보상 골드, 경험치 지급
    {
        DataContainer.sInst.PlayerStatus().EXP = +exp;

        System.Random rand = new System.Random();
        int dropGoldRandValue = rand.Next(dropGold - 10, dropGold + 10);

        if(dropGoldRandValue > 0)
        {
            DataContainer.sInst.Inventory().Gold = +dropGoldRandValue;
        }
    }
}

[Serializable]
public class DropItem
{
    public ItemObject item; // 드랍 아이템
    public float probability; // 드랍될 확률
}