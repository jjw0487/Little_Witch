using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IPlayer value))
        {
            particle.transform.position = value.Position() + new Vector3(0f, 0.8f, 0f);
            particle.Play();
            value.GetHit(30f);
        }
    }
}
