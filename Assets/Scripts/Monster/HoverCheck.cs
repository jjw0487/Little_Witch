using UnityEngine;

public class HoverCheck : MonoBehaviour
{
    private void OnMouseEnter()
    {
        this.GetComponent<Renderer>().material.SetFloat("_UseEmission", 1.0f);
    }
    private void OnMouseExit()
    {
        this.GetComponent<Renderer>().material.SetFloat("_UseEmission", 0.0f);
    }
}
