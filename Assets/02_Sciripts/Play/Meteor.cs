using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using n_O2Gauge;

public class Meteor : MonoBehaviour
{

    //private bool isInMeteor;
    //public Transform headFose;
    //public float startcheckRadius;
    //public LayerMask whatisPlayer;

    private Vector3 spawnPos;

    private void Start()
    {
        spawnPos = transform.position;
    }

    void Update()
    {
        //isInMeteor = Physics2D.OverlapCircle(headFose.position, startcheckRadius, whatisPlayer);

        //Debug.Log(isInMeteor);

        //if (!isInMeteor) { return; }
        transform.Translate(Vector3.left * Time.deltaTime * 10f, Space.Self);

        if(transform.position.x < (spawnPos.x - 60f))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            O2.DamageO2(10);
            Destroy(gameObject);
        }
    }
}
