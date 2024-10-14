using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisturbingSightObject : MonoBehaviour
{
    [SerializeField] private GameObject dsObjs;

    public void IsDisturbing(bool active)
    {
        dsObjs.gameObject.SetActive(!active);
    }
}
