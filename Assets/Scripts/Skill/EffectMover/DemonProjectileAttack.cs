using UnityEngine;

public class DemonProjectileAttack : ProjectileMover
{ // DemonAttack
    [SerializeField] private AudioClip sound;
    private Vector3 dir;
    private float power;
    public void Initialize(Vector3 target, float _power)
    {
        dir = (target - this.transform.position).normalized;
        power = _power;
    }
    protected override void Start()
    {
        base.Start();
    }

    private void FixedUpdate()
    {
        if (speed != 0)
        {
            rb.velocity = dir * speed;
            //transform.position += targetDir * (speed * Time.deltaTime);
        }
    }


    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out IPlayer value))
        {
            value.GetHit(power);
        }

        SoundManager.sInst.Play(sound, 0.5f);

        base.OnCollisionEnter(collision);
    }


}

