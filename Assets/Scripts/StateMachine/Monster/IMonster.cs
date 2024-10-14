using UnityEngine;

/// <summary>
/// Monster���� ����� �ൿ �Լ�
/// </summary>
public interface IMonster
{
    void Spawn(Vector3 spawnPos); // ������Ʈ Ǯ������ ã�ƿ� ����
    void Despawn(); // ���� ���� �ٽ� ������Ʈ Ǯ���� ����
    void GetHit(float dmg); // �ǰ� ����
    void Debuff(float duration, float percentage); // ����� ����
    void ChangeTarget(IPlayer _target); // ���� Ÿ�� ����
    bool IsAlive(); 
    Vector3 Position();
    Vector3 HitPoint();
}
