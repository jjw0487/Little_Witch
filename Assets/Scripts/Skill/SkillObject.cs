using UnityEngine;

/// <summary>
/// ������Ʈ Ǯ���Ǿ� ���Ǵ� ��ų ����Ʈ�� �߻�ȭ�� �θ�
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
