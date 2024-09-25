using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] protected ItemData data;
    [SerializeField] protected bool isPoolable = true;

    // ������ ȹ�� �� �κ��丮�� �߰��� ����
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
        timer = 120f; // �������� ȹ������ ������ 2�� �Ŀ� �Ҹ��
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
