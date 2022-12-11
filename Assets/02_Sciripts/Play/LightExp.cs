using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightExp : MonoBehaviour
{
    private Light2D light2D;
    private Rigidbody2D rb;
    private bool objStop = true;

    private Vector3 initPos;

    private void Awake()
    {
        initPos = transform.position;
        light2D = GetComponent<Light2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if(objStop == false)
        {
            Vector2 newPos = Vector2.MoveTowards(rb.position, Vector2.one * 40f, 5f * Time.deltaTime);
            rb.MovePosition(newPos);
        }
    }

    private void OnEnable()
    {
        //AddForce

    }

    public void TurnOn(float duration)
    {
        objStop = true;
        //light2D.gameObject.SetActive(true);
        Invoke("TurnOff", duration);
    }

    private void TurnOff()
    {
        light2D.gameObject.SetActive(false);
        transform.position = initPos;
    }
}
