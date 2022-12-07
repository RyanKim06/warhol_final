using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcicleB : MonoBehaviour
{
    public Vector3 targetPos;
    public float upSpeed;
    public float downSpeed;
    public float upDownDealy;
    public float delay;

    private Vector3 firstPos;

    private void Awake()
    {
        firstPos = transform.position;
    }

    private void Start()
    {
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while(true)
        {
            while (transform.position.y < firstPos.y + targetPos.y)
            {
                transform.position = Vector2.MoveTowards(transform.position, transform.position + targetPos, Time.deltaTime * upSpeed);
                yield return null;
            }

            yield return new WaitForSeconds(upDownDealy);

            while (transform.position.y > firstPos.y)
            {
                transform.position = Vector2.MoveTowards(transform.position, transform.position - targetPos, Time.deltaTime * downSpeed);
                yield return null;
            }

            yield return new WaitForSeconds(delay);
        }
    }
}
