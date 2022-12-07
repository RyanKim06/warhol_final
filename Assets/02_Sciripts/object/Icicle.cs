using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using n_O2Gauge;

public class Icicle : MonoBehaviour
{
    public float upDownSize;
    public float upDownDelay; //up - upDownDelay - down
    public float afterDelay; //up - upDownDelay - down - afterDelay - up2...
    public float speed;
    public float damage;

    private bool upOrDown = false; //up: true, down: false
    private SpriteRenderer sr;
    private Vector2 currentVelocity = Vector2.zero;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if(upDownSize == 0)
            upDownSize = sr.sprite.bounds.size.y;

        //print(meshYSize);

        StartCoroutine(UpDown());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            O2.DamageO2(damage);
        }
    }

    private void Move()
    {
        if (upOrDown) //up
        {
            //Debug.Log($"{upOrDown}, true == up");

            //transform.position =
            //    Vector2.SmoothDamp(transform.position, new Vector2(transform.position.x, transform.position.y + meshYSize), ref currentVelocity, 1f);

            StartCoroutine(lerpCoroutine(transform.position, transform.position + new Vector3(0f, upDownSize), 1f));
        }
        else //down
        {
            //Debug.Log($"{upOrDown}, false == down");

            //transform.position =
            //    Vector2.MoveTowards(transform.position,
            //    new Vector2(transform.position.x, transform.position.y - meshYSize),
            //    Time.deltaTime * speed);

            StartCoroutine(lerpCoroutine(transform.position, transform.position - new Vector3(0f, upDownSize), 0.4f));
        }
    }

    IEnumerator UpDown()
    {
        while (true)
        {
            upOrDown = !upOrDown;
            Move();
            yield return new WaitForSeconds(upDownDelay);

            upOrDown = !upOrDown;
            Move();
            yield return new WaitForSeconds(afterDelay);

            yield return null;
        }
    }

    IEnumerator lerpCoroutine(Vector2 current, Vector2 target, float time)
    {
        float elapsedTime = 0f;

        transform.position = current;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;

            transform.position = Vector2.Lerp(current, target, elapsedTime / time);

            yield return null;
        }
    }

}
