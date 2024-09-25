using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttack : Projectile
{   
    [SerializeField] protected ParticleSystem trail;

    private SkillReferenceData data;
    private Vector3 direction;
    private float playerSP;
    private bool isWorking;
    private float timer;

    public override void Initialize(SkillReferenceData _data)
    {
        isWorking = false;
        data = _data;

        this.gameObject.SetActive(false);
    }

    protected void FixedUpdate()
    {
        if (isWorking)
        {
            transform.position += direction * speed * Time.deltaTime;
        }

        if(timer > 0f)
        {
            timer -= Time.deltaTime;

            if(timer <= 0f)
            {
                Despawn();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isWorking) return;

        isWorking = false;

        hit.gameObject.SetActive(true);

        trail.Stop();
        trail.gameObject.SetActive(false);

        Collider[] overlap = Physics.OverlapSphere(this.transform.position, data.OverlapRadius);

        foreach (Collider col in overlap)
        {
            if(col.gameObject.layer == 8)
            {
                if (col.transform.parent.TryGetComponent(out IMonster value))
                {
                    if (value.IsAlive())
                    {
                        value.GetHit(data.Damage + playerSP);
                    }
                }
            }
        }
    }

    

    public override void Spawn(Vector3 _startPos, Vector3 _target,
        Transform player, float _sp)
    {
    
        isWorking = true;

        this.transform.position = _startPos;

        direction = (_target - _startPos).normalized;

        playerSP = _sp;

        trail.gameObject.SetActive(true);

        //Debug.Log($"start position : {_startPos}, target position : {target}");

        this.gameObject.SetActive(true);

        SoundManager.sInst.Play(sound);

        timer = duration;

    }
    public override void Despawn()
    {
        isWorking = false;

        trail.gameObject.SetActive(false);
        hit.gameObject.SetActive(false);

        this.gameObject.SetActive(false);
    }

    public override bool IsWorking() => this.gameObject.activeSelf;
}
