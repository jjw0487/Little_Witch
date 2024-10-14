using UnityEngine;

/// <summary>
/// ���� �ִϸ��̼� �۾� �� ���������� �Ϻ��� �۾����� �ʰ� �����Ͽ� �ش� ��ũ��Ʈ�� ����
/// Monster.cs �� �����Ͽ� �����Ѵ�. ���߿� �ִϸ��̼� �۾� ���� �ϸ鼭 ������� ��
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
