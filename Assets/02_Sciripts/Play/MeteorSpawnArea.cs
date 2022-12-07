using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawnArea : MonoBehaviour
{
    public GameObject []meteor;

    IEnumerator SpawnMeteor()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);

            Instantiate(meteor[Random.Range(0, meteor.Length)], transform.position + new Vector3(7f, 5f), transform.rotation);
            yield return new WaitForSeconds(2f);

            Instantiate(meteor[Random.Range(0, meteor.Length)], transform.position + new Vector3(7f, -5f), transform.rotation);
            yield return new WaitForSeconds(1f);

            Instantiate(meteor[Random.Range(0, meteor.Length)], transform.position + new Vector3(7f, 0f), transform.rotation);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
            StartCoroutine(SpawnMeteor());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            StopCoroutine(SpawnMeteor());
    }
}
