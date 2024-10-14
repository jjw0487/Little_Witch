using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

/// <summary>
/// 필드 몬스터 스포너
/// </summary>
public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private Monster target; // 스폰 몬스터
    [SerializeField] private int maxMonsterCount; // 필드 내 존재 가능한 개체수
    [SerializeField] private Transform[] spawnPoint; // 스폰 위치
    [SerializeField] private Transform monsterParent; 

    protected CancellationTokenSource source;
    private MonsterObjectPooler pooler;
    private int spawnedMonsterCount = 0;

    private System.Random rand = new System.Random();

    private bool isSpawning;
    private void OnDisable()
    {
        source.Cancel();
        source.Dispose();
    }

    private void Start()
    {
        source = new CancellationTokenSource();

        pooler = new MonsterObjectPooler(target,
            maxMonsterCount, monsterParent, DeathCount);

        for (int i = 0; i < maxMonsterCount; i++)
        {
            SpawnMonster();
        }
    }

    private void SpawnMonster()
    {
        spawnedMonsterCount++;

        Monster monster = pooler.GetObj();

        int randNum = rand.Next(0, spawnPoint.Length);

        monster.Spawn(spawnPoint[randNum].position);
    }

    private void DeathCount() // 몬스터 사망 시 Callback으로 DeathCount()를 실행하여 Update문 없이 개체수 관리 가능
    {
        spawnedMonsterCount--;
        
        if(!isSpawning)
        {
            Spawn(5f).Forget();
        }
    }

    private async UniTaskVoid Spawn(float delay) // maxMonsterCount를 확인하여 필요 시 부족한 만큼 몬스터를 소환
    {
        isSpawning = true;

        await UniTask.Delay(TimeSpan.FromSeconds(delay),
            cancellationToken: source.Token);

        if(spawnedMonsterCount < maxMonsterCount)
        {
            SpawnMonster();
            Spawn(1f).Forget();
        }
        else
        {
            isSpawning = false;
        }
    }
}
