using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    public GameObject spawnPrefab;
    public Vector3 spawnOffset;
    public float spawnDelay;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            Invoke("Spawn", spawnDelay);
    }

    private void Spawn()
    {
        Instantiate(spawnPrefab, transform.position + spawnOffset, Quaternion.identity);
        gameObject.SetActive(false);
    }
}
