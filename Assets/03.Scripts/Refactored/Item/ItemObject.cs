using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] protected ItemData data;
    [SerializeField] protected bool isPoolable = true;

    // 아이템 획득 시 인벤토리에 추가될 수량
    protected int value;
    protected bool isSpawned = false;
    protected float timer;

    public int GetItemId() => data.ItemId;
    public bool IsSpawned() => isSpawned;
    public bool IsPoolable() => isPoolable;

    private void FixedUpdate()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                Despawn();
            }
        }
    }
    public virtual void Spawn(Vector3 _pos, int _value)
    {
        isSpawned = true;
        value = _value;
        timer = 120f; // 아이템은 획득하지 않으면 2분 후에 소멸됨
        this.transform.position = new Vector3(_pos.x, _pos.y + 1f, _pos.z);
        this.gameObject.SetActive(true);
    }

    public virtual void Despawn()
    {
        if(!isPoolable)
        {
            Destroy(this.gameObject);
            return;
        }

        isSpawned = false;
        this.gameObject.SetActive(false);
    }

    public virtual void ItemPickedUp()
    {
        if (DataContainer.sInst.Inventory().Acquire(data, value))
        {
            Despawn();
        }
    }

}
