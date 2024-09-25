using UnityEngine;
using Enums;

public abstract class ItemData : Configurable
{
    [SerializeField] private ItemType type;
    public ItemType Type => type;

    [SerializeField] private int itemId;
    public int ItemId => itemId;

    [SerializeField] private Sprite sprite;
    public Sprite Sprite => sprite;

    [SerializeField] private string itemName;
    public string ItemName => itemName;

    [SerializeField] private string optionDesc;
    public string OptionDesc => optionDesc;

    [TextArea(3, 10)]
    [SerializeField] private string description;
    public string Description => description;

    public virtual MaterialItemType GetMaterialType() => MaterialItemType.None;
    public virtual ConsumableItemType GetConsumableType() => ConsumableItemType.None;
    public virtual InteractableItemType GetInteractableType() => InteractableItemType.None;
    public virtual EquipmentItemType GetEquipmentType() => EquipmentItemType.None;
    public virtual AdditionalStatType GetAdditionalStatType() => AdditionalStatType.None;
    public abstract int GetOptionValue();
    public abstract int GetCurrencyWhenBuy();
    public abstract int GetCurrencyWhenSell();


}
