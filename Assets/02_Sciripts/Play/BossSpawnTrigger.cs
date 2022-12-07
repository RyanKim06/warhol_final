using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnTrigger : MonoBehaviour
{
    public GameObject bossManagerObject;

    private BossManager bossManager;

    private void Awake()
    {
        bossManager = bossManagerObject.GetComponent<BossManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            bossManager.SpawnBoss();
    }
}
