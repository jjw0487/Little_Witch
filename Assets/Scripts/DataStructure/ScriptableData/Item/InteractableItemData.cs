using Enums;
using UnityEngine;
[CreateAssetMenu(fileName = "InteractableItemData",
    menuName = "ItemData/InteractableItemData", order = 4)]
public class InteractableItemData : ItemData
{
    [SerializeField] private InteractableItemType detail; // ��� Ÿ��

    public override InteractableItemType GetInteractableType()
    {
        return detail;
    }

    public override int GetOptionValue()
    {
        return 0;
    }

    public override int GetCurrencyWhenBuy()
    {
        return 0;
    }

    public override int GetCurrencyWhenSell()
    {
        return 0;
    }

    
}
