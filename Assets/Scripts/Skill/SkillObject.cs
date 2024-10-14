using UnityEngine;

/// <summary>
/// 오브젝트 풀링되어 사용되는 스킬 이펙트의 추상화된 부모
/// </summary>
public abstract class SkillObject : MonoBehaviour
{
    [SerializeField] protected AudioClip sound;
    public abstract void Initialize(SkillReferenceData _data);
    public abstract void Spawn(Vector3 _startPos, Vector3 _target, 
        Transform player, float _sp);
    public abstract void Despawn();
    public abstract bool IsWorking();
}
