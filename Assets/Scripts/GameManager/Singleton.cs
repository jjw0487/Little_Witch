using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T inst = null;
    public static T sInst
    {
        get
        {
            if (inst == null)
            {
                inst = FindObjectOfType(typeof(T)) as T;

                return inst;
            }
            return inst;
        }
    }
}