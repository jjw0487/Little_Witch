using UnityEngine;

/// <summary>
/// 과거 애니메이션 작업 전 계층구조를 완벽히 작업하지 않고 진행하여 해당 스크립트를 거쳐
/// Monster.cs 에 접근하여 동작한다. 나중에 애니메이션 작업 새로 하면서 고쳐줘야 함
/// </summary>
public class MonsterNode : MonoBehaviour, IMonster
{
    [SerializeField] private Monster node;
    public void ChangeTarget(IPlayer _target) => node.ChangeTarget(_target);
    public void Debuff(float duration, float percentage) => node.Debuff(duration, percentage);
    public void Despawn() => node.Despawn();
    public void GetHit(float dmg) => node.GetHit(dmg);
    public Vector3 HitPoint() => node.HitPoint();
    public bool IsAlive() => node.IsAlive();
    public Vector3 Position() => node.Position();
    public void Spawn(Vector3 spawnPos) => node.Spawn(spawnPos);
}
