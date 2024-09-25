using UnityEngine;

public class WorldToScreenObjectManager : MonoBehaviour
{
    [SerializeField] private PoolUnit<MonsterHPSlider> hpSlider;
    [SerializeField] private PoolUnit<FloatingDamage> floatingDamage;
    [SerializeField] private PoolUnit<ItemAcquiredNotification> itemNotification;

    private MonsterHPSliderPooler hpPool;
    private FloatingDamagePooler dmgPool;


    private void OnDisable()
    {
        EventManager.monsterHPEvent -= hpPool.GetObj;
        EventManager.floatingDamageEvent -= SpawnFloatingDamage;
    }


    void Start()
    {
        InitializeWorldToScreenObjectManager();
    }

    private void InitializeWorldToScreenObjectManager()
    {
        hpPool = new MonsterHPSliderPooler(hpSlider.unit, 
            hpSlider.amount, hpSlider.parent);

        EventManager.monsterHPEvent += hpPool.GetObj;

        dmgPool = new FloatingDamagePooler(floatingDamage.unit, 
            floatingDamage.amount, floatingDamage.parent);

        EventManager.floatingDamageEvent += SpawnFloatingDamage;
    }

    private void SpawnFloatingDamage(Vector3 pos, float dmg)
    {
        dmgPool.GetObj().Spawn(pos, dmg);
    }


    
}
