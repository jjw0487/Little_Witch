using UnityEngine;

public class SpinningLight : SkillObject
{
    [SerializeField] private LightUnit[] units;
    [SerializeField] private float duration;

    private SkillReferenceData data;
    private Transform player;
    private float playerSP;
    private bool isWorking;
    private float timer;

    public override bool IsWorking() => isWorking;

    private void FixedUpdate()
    {
        if(isWorking)
        {
            transform.position = player.position;
            transform.RotateAround(player.position, Vector3.up, -120f * Time.deltaTime);
        }

        if (timer > 0f)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                if (isWorking) Despawn();
            }
        }
    }
    public override void Initialize(SkillReferenceData _data)
    {
        isWorking = false;
        data = _data;

        this.gameObject.SetActive(false);
    }

    public override void Spawn(Vector3 startPos, Vector3 _target,
        Transform _player, float _sp)
    {
        player = _player;

        isWorking = true;

        playerSP = _sp;

        transform.position = player.position;

        for (int i = 0; i < units.Length; i++) 
        {
            units[i].Spawn(data.Damage + playerSP);
        }

        this.gameObject.SetActive(true);

        timer = duration;
    }

    public override void Despawn()
    {
        isWorking = false;

        this.gameObject.SetActive(false);
    }

    


}
