using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayManager : MonoBehaviour
{
    public static PlayManager inst;
    private void Awake() => inst = this;
    public abstract PlayerController GetPlayer();
    public abstract InteractionManager Interact();
    public abstract SkillManager Skill();
    public abstract int GetPlayerLevel();
}
