using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSington<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T instance;
    
    protected virtual void Awake()
    {
        if (instance == null)
            instance = this as T;
    }
}
