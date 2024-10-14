using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ConsumableItemData",
    menuName = "ItemData/ConsumableItemData", order = 4)]
public class ConsumableItemData : ItemData
{
    [SerializeField] private ConsumableItemType detail; // 장비 타입
    [SerializeField] public int optionValue; // 장비 능력치
    [SerializeField] private int currency_Buy;
    [SerializeField] private int currency_Sell;

    public override ConsumableItemType GetConsumableType()
    {
        return detail;
    }

    public override int GetOptionValue()
    {
        return optionValue;
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
