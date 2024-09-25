using UnityEngine;

public interface IMonster
{
    void Spawn(Vector3 spawnPos);
    void Despawn();
    void GetHit(float dmg);
    void Debuff(float duration, float percentage);
    void ChangeTarget(IPlayer _target);
    bool IsAlive();
    Vector3 Position();
    Vector3 HitPoint();

}
