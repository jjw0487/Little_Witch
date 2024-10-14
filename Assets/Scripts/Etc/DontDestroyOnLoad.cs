using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : Singleton<DontDestroyOnLoad>
{
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
