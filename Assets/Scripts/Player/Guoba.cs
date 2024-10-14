using System.Collections.Generic;
using UnityEngine;

public class Guoba : SkillObject, IPlayer
{
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private GameObject explosion;
    [SerializeField] private float duration;

    private SkillReferenceData data;

    private bool isWorking;
    private float playerSP;
    private float hp;
    private float timer;
    private IPlayer player;
    private List<IMonster> target = new List<IMonster>();

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
    public Vector3 Position() => this.transform.position;
    public bool IsDead() => false;
    public override void Initialize(SkillReferenceData _data)
    {
        isWorking = false;
        data = _data;
        this.gameObject.SetActive(false);
    }
    public override void Spawn(Vector3 _startPos, Vector3 _target, Transform _player, float _sp)
    {
        isWorking = true;

        SoundManager.sInst.Play(sound);

        this.transform.position = _target + data.PerformOffset;

        target.Clear();

        if(player == null)
        {
            player = _player.GetComponent<IPlayer>();
        }

        playerSP = _sp;

        timer = duration;

        hp = 80 + data.Level * 7;

        this.gameObject.SetActive(true);

        ChangeTarget();
    }
    public override void Despawn()
    {
        isWorking = false;

        // ���� �� ������ ������
        Explosion();

        // ����Ʈ�� ��Ƶ� Ÿ���� ������ ������Ʈ���� ������� ����
        if(player != null)
        {
            for (int i = 0; i < target.Count; i++)
            {
                if (Vector3.Distance(target[i].Position(), player.Position()) < 10f)
                {
                    target[i].ChangeTarget(player);
                }
                else
                {
                    target[i].ChangeTarget(null);
                }
            }
        }

        this.gameObject.SetActive(false);
    }

    public void GetHit(float dmg)
    {
        hp -= dmg;

        if (hp <= 0) Despawn();
    }

    private void ChangeTarget()
    {
        // ���Ǹ� ���� ���� �����ÿ� Ÿ���� �� ������Ʈ�� ����

        Collider[] overlap = Physics.OverlapSphere(this.transform.position, data.OverlapRadius);

        foreach (Collider col in overlap)
        {
            if (col.gameObject.layer == 8)
            {
                Debug.Log("collected : " + col.name);

                if (col.transform.parent.TryGetComponent(out IMonster value))
                {
                    if (value.IsAlive())
                    {
                        target.Add(value);
                        value.ChangeTarget(this);
                    }
                }
            }
        }
    }

    private void Explosion()
    {
        SoundManager.sInst.Play(explosionSound);

        // ���� ����Ʈ ����
        var obj = Instantiate(explosion, this.transform.position, Quaternion.identity);

        Destroy(obj, 1.2f);

        Collider[] overlap = Physics.OverlapSphere(this.transform.position, 2f);

        foreach (Collider col in overlap)
        {
            if (col.gameObject.layer == 8)
            {
                Debug.Log("collected : " + col.name);

                if (col.transform.parent.TryGetComponent(out IMonster value))
                {
                    if (value.IsAlive())
                    {
                        value.GetHit(data.Damage + playerSP); // ���� ������
                    }
                }
            }
        }
    }

    
}
