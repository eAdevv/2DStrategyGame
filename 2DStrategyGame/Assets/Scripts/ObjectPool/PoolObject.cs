using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    public string poolKey;

    public virtual void OnAwake()
    {
        //Debug.Log("PoolObject Awake");
    }

    protected void Done()
    {
        PoolManager.Instance.ReturnObjectToQueue(gameObject);
    }

    
}