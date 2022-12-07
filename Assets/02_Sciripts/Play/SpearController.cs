using n_O2Gauge;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearController : MonoBehaviour
{
    public bool collisionEntered = false;

    private SpriteRenderer spearSprite;

    private void Awake()
    {
        spearSprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnDisable()
    {
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.identity;
        collisionEntered = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //player detected
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("spearcontroller, detected player");
            collisionEntered = true;
            transform.position = collision.transform.position;
            O2.DamageO2(30);
            StartCoroutine(FadeInDeactive());
        }
        else if(collision.gameObject.CompareTag("round"))
        {
            Debug.Log("spearcontroller, detected round");
            collisionEntered = true;
            StartCoroutine(FadeInDeactive());
        }
    }

    public IEnumerator FadeInDeactive()
    {
        Color c = spearSprite.color;
        for (float f = 1f; f >= 0; f -= 0.005f)
        {
            c.a = f;
            spearSprite.color = c;
            yield return null;
        }
        gameObject.SetActive(false);
        spearSprite.color = new Color(c.r, c.g, c.b, 1f);
    }
}