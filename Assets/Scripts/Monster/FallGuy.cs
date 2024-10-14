using System.Collections;
using UnityEngine;

public class FallGuy : Monster
{







    // 마을몬스터, 죽을 때 카운트를 하여 스폰되고 퀘스트를 진행한다.

/*
    protected override IEnumerator DelayDead(float chill)
    {
        MonsterSpawner.Inst.KilledMonsterCounter(this.monStat.orgData.monsterName, this.gameObject);
        nav.SetDestination(transform.position);
        anim.SetTrigger("Death");
        isDead = true;
        while (chill > 0.0f)
        {
            chill -= Time.deltaTime;
            yield return null;
        }
        int rndNum = UnityEngine.Random.Range(0, 11); // 난수를 생성해서 아이템 랜덤하게 드랍
        if (rndNum < 5 && monStat.orgData.dropItems[0] != null)
        {
            GameObject DropItem = Instantiate(monStat.orgData.dropItems[0].obj,
                this.transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity); // 드랍 아이템
            DropItem.transform.SetParent(SceneData.Inst.ItemPool);
        }
        else if (rndNum == 1 && monStat.orgData.dropItems[0] != null)
        {
            GameObject DropItem = Instantiate(monStat.orgData.dropItems[1].obj,
                this.transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity); // 드랍 아이템
            DropItem.transform.SetParent(SceneData.Inst.ItemPool);
        }
        Destroy(gameObject);
    }*/
}
