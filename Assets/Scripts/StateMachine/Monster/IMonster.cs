using UnityEngine;

/// <summary>
/// Monster들의 공통된 행동 함수
/// </summary>
public interface IMonster
{
    void Spawn(Vector3 spawnPos); // 오브젝트 풀러에서 찾아와 생성
    void Despawn(); // 죽은 몬스터 다시 오브젝트 풀러에 저장
    void GetHit(float dmg); // 피격 당함
    void Debuff(float duration, float percentage); // 디버프 당함
    void ChangeTarget(IPlayer _target); // 현재 타겟 변경
    bool IsAlive(); 
    Vector3 Position();
    Vector3 HitPoint();
}
