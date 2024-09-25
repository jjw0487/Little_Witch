using UnityEngine;
using UnityEngine.AI;

public class CharacterProperty : MonoBehaviour
{
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected Animator anim;
    [SerializeField] protected NavMeshAgent nav;
}
