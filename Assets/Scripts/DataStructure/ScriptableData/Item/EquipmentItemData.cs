using UnityEngine;
using Enums;
[CreateAssetMenu(fileName = "EquipmentItemData", 
    menuName = "ItemData/EquipmentItemData", order = 4)]
public class EquipmentItemData : ItemData
{
    [SerializeField] private EquipmentItemType detail; // 장비 타입
    [SerializeField] private AdditionalStatType additionalStatType; // 장비 타입
    [SerializeField] public int optionValue; // 장비 능력치
    [SerializeField] private int currency_Buy;
    [SerializeField] private int currency_Sell;

    public override EquipmentItemType GetEquipmentType() => detail;
    public override AdditionalStatType GetAdditionalStatType() => additionalStatType;
    public override int GetOptionValue() => optionValue;
    public override int GetCurrencyWhenBuy() => currency_Buy;
    public override int GetCurrencyWhenSell() => currency_Sell;
}
