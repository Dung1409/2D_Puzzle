using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _intance;
    public static T intance => _intance;

    public virtual void Awake()
    {
        if (_intance == null)
        {
            _intance = this as T;
        }
        else
        {
            Destroy(this as T);
        }
    }
}
