using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : SkillObject
{
    [SerializeField] protected float speed;
    [SerializeField] protected float hitOffset;
    [SerializeField] protected float duration;

    [SerializeField] protected ParticleSystem hit;
}
