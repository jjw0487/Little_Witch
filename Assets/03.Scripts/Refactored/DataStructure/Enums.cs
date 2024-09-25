namespace Enums
{
    public enum NpcType
    {
        Aranara, Kid, Panda, Sara, StrayCat, Structure
    }

    public enum QuestType
    {
          Hunting, Delivery, Collect, TalkTo
    }

    public enum RewardType
    {
        Item, Gold, Exp
    }

    public enum SkillKey
    { 
        NormalAttack,
        Guoba,
        ShockWave,
        FrozenSmash,
        ThunderSpike,
        SpinningLight,
        Firework
    }
    public enum AdditionalStatType
    {
        None, SP, DP, MaxHP, MaxMp
    }

    
    public enum StateType
    {
        Init,
        Idle,
        Move,
        Attack,
        GetHit,
        Pending,
        Die
    }

    public enum SkillType
    { Buff, Attck, Debuff, AttackNDebuff, NormalAttack }


    public enum PlayModeType
    { 
        None, Street, Broom, Interact, Dead
    }

    public enum LayerType
    {
        Player,
        UI,
        Obstcle,
        Monster,
        Ground,
        SkillRange
    }
    public enum ItemType 
    { 
        Consumable, Material, Interactable, Equipment
    }
    public enum ConsumableItemType
    {
        None, HP, MP, ST, AP
    }
    public enum EquipmentItemType
    {
        None, Weapon, Necklas, Ring
    }
    public enum MaterialItemType
    {
        None, Quest
    }
    public enum InteractableItemType
    {
        None, Reward
    }
    public enum SlotType
    {
        Item, Equipment, SkillSet, QuickSlot
    }
}
