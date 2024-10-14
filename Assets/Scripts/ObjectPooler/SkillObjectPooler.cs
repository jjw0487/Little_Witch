using UnityEngine;

public class SkillObjectPooler : UnitObjectPooler<SkillObject>
{
    public SkillObjectPooler(SkillObject prefab, int initAmount,
        Transform parent, SkillReferenceData _data)
        :base (prefab, initAmount, parent)
    {
        data = _data;
        CreatePool();
    }

    private SkillReferenceData data;

    public override SkillObject CreateObj()
    {
        var obj = Object.Instantiate(prefab);
        obj.Initialize(data);
        obj.transform.SetParent(parent);
        obj.name = data.Trigger;
        return obj;
    }

    public override SkillObject GetObj()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].IsWorking())
            {
                return pool[i];
            }
        }

        var obj = CreateObj();

        pool.Add(obj);

        return obj;
    }
}
