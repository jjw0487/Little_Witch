using UnityEngine;

public interface IPlayer
{
    void GetHit(float dmg);
    Vector3 Position();
    bool IsDead();
}
