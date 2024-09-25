using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private Monster target;
    [SerializeField] private int maxMonsterCount;
    [SerializeField] private Transform[] spawnPoint;
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

    private void DeathCount()
    {
        spawnedMonsterCount--;
        
        if(!isSpawning)
        {
            Spawn(5f).Forget();
        }
    }

    private async UniTaskVoid Spawn(float delay)
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
