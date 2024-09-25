using UnityEngine;

/// <summary>
/// 다른 아이템의 정보를 가지고 있다 이벤트가 발생하면 가지고 있던 아이템을 본인 위치에 드랍한다.
/// </summary>

public class ChestItem : ItemObject
{
    [SerializeField] private ItemObject[] dropItems;
    [SerializeField] private GameObject dust;

    public override void ItemPickedUp()
    {
        DropItem();
    }

    private void DropItem()
    {
        SoundManager.sInst.Play("OpenChestItem");

        Vector3 pos = this.transform.position;

        pos.y += 0.5f;

        var obj = Instantiate(dust, pos, Quaternion.identity);

        Destroy(obj, 1.2f);

        for (int i = 0; i < dropItems.Length; i++)
        {
            if (EventManager.itemSpawnEvent != null)
            {
                EventManager.itemSpawnEvent(this.transform.position, 1, dropItems[i]);
            }
        }

        Despawn();
    }

    private void OnMouseEnter()
    {
        if (this.GetComponent<Renderer>())
        {
            this.GetComponent<Renderer>().material.SetFloat("_UseEmission", 1.0f);
        }

    }
    private void OnMouseExit()
    {
        if (this.GetComponent<Renderer>())
        {
            this.GetComponent<Renderer>().material.SetFloat("_UseEmission", 0.0f);
        }
    }
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DropItem();
        }
    }

    
}
