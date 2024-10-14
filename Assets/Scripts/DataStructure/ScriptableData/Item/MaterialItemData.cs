using Enums;
using UnityEngine;

[CreateAssetMenu(fileName = "MaterialItemData",
    menuName = "ItemData/MaterialItemData", order = 4)]
public class MaterialItemData : ItemData
{
    [SerializeField] private MaterialItemType detail; // 장비 타입
    [SerializeField] private int currency_Buy;
    [SerializeField] private int currency_Sell;

    public override MaterialItemType GetMaterialType()
    {
        return detail;
    }

    public override int GetOptionValue()
    {
        return 0;
    }

    public override int GetCurrencyWhenBuy()
    {
        return currency_Buy;
    }

    public override int GetCurrencyWhenSell()
    {
        return currency_Sell;
    }

    
}
