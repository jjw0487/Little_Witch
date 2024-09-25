using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDestinationPoint : MonoBehaviour
{
    [SerializeField] private ParticleSystem part;

    public void SpotPoint(Vector3 pos)
    {
        this.transform.position = pos;
        part.Play();
    }
}
