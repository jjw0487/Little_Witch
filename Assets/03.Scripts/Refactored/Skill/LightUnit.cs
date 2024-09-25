using UnityEngine;

public class LightUnit : MonoBehaviour
{
    [SerializeField] private AudioClip sound;

    [SerializeField] private GameObject explosion;

    [SerializeField] private Vector3 initialPosition;

    private float totalDamage;

    public void Spawn(float _totalDamage)
    {
        totalDamage = _totalDamage;

        this.transform.localPosition = initialPosition;

        this.gameObject.SetActive(true);
    }

    public void Despawn()
    {
        this.gameObject.SetActive(false);
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            Explosion();

            Collider[] overlap = Physics.OverlapSphere(this.transform.position, 0.5f);

            foreach (Collider col in overlap)
            {
                if(col.isTrigger) continue;

                Debug.Log("collected : " + col.name);

                if (col.transform.parent.TryGetComponent(out IMonster value))
                {
                    if (value.IsAlive())
                    {
                        value.GetHit(totalDamage);
                    }
                }
            }

            Despawn();
        }
    }

    private void Explosion()
    {
        SoundManager.sInst.Play(sound);

        var obj = Instantiate(explosion, this.transform.position, Quaternion.identity);

        Destroy(obj, 1.2f);
    }
}