using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Waterdrop : MonoBehaviour
{
    private IObjectPool<Waterdrop> managedPool;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("waterdrop collisionEntered");
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag=="Boss1")
        {
            //Debug.Log("BOSS DAMAGED");
            DestroyWaterdrop();
        }
    }

    public void SetWaterdropPool(IObjectPool<Waterdrop> pool)
    {
        managedPool = pool;
    }
    public void DestroyWaterdrop()
    {
        managedPool.Release(this);
    }
}
