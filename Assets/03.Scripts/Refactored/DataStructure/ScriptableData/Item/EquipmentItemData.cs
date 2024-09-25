using UnityEngine;
using Enums;
[CreateAssetMenu(fileName = "EquipmentItemData", 
    menuName = "ItemData/EquipmentItemData", order = 4)]
public class EquipmentItemData : ItemData
{
    [SerializeField] private EquipmentItemType detail; // ��� Ÿ��
    [SerializeField] private AdditionalStatType additionalStatType; // ��� Ÿ��
    [SerializeField] public int optionValue; // ��� �ɷ�ġ
    [SerializeField] private int currency_Buy;
    [SerializeField] private int currency_Sell;

    public override EquipmentItemType GetEquipmentType() => detail;
    public override AdditionalStatType GetAdditionalStatType() => additionalStatType;
    public override int GetOptionValue() => optionValue;
    public override int GetCurrencyWhenBuy() => currency_Buy;
    public override int GetCurrencyWhenSell() => currency_Sell;
}
