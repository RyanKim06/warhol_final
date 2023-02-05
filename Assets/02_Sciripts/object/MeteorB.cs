using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using n_O2Gauge;

public class MeteorB : MonoBehaviour
{
    //오브젝트 풀
    private IObjectPool<MeteorB> managedPool;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            O2.DamageO2(2);
            DestroyMeteor();
        }
    }

    public void SetMeteorPool(IObjectPool<MeteorB> pool)
    {
        managedPool = pool;
    }
    public void DestroyMeteor()
    {
        managedPool.Release(this);
    }
}
