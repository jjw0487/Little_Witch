using UnityEngine;

public class FrozenSmash : SkillObject
{
    [SerializeField] private float duration;

    private SkillReferenceData data;
    private float playerSP;
    private bool isWorking;
    private float timer;

    protected void FixedUpdate()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                if (isWorking) Despawn();
            }
        }
    }
    public override bool IsWorking() => isWorking;
    public override void Initialize(SkillReferenceData _data)
    {
        isWorking = false;
        data = _data;
        this.gameObject.SetActive(false);
    }

    public override void Spawn(Vector3 _startPos, Vector3 _target,
        Transform player, float _sp)
    {
        isWorking = true;

        SoundManager.sInst.Play(sound, 0.5f);

        this.transform.position = _target + data.PerformOffset;

        playerSP = _sp;

        timer = duration;

        this.gameObject.SetActive(true);

        Collider[] overlap = Physics.OverlapSphere(this.transform.position, data.OverlapRadius);

        foreach (Collider col in overlap)
        {
            if (col.gameObject.layer == 8)
            {
                if (col.transform.parent.TryGetComponent(out IMonster value))
                {
                    if (value.IsAlive())
                    {
                        value.GetHit(data.Damage + playerSP);
                        value.Debuff(data.DebuffDuration, data.DebuffPercentage);
                    }
                }
            }
        }

    }
    public override void Despawn()
    {
        isWorking = false;

        this.gameObject.SetActive(false);
    }

   
}
