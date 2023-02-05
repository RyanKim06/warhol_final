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

    public bool isDelay = false;
    public int fwdDir = 1;

    private void Start()
    {
        initPos = transform.position;
        light2D = GetComponent<Light2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        objStop = false;
    }

    private void FixedUpdate()
    {
        if(objStop == false)
        {
            Vector2 newPos = Vector2.MoveTowards(transform.position, transform.position + (new Vector3(fwdDir, 1) * 40f), 5f * Time.deltaTime);
            transform.position = newPos;
        }
    }

    public void TurnOn(float duration)
    {
        Debug.Log("LightExp TurnOn()");
        rb.gravityScale = 0f;
        objStop = true;
        isDelay = true;

        Color onColor;
        ColorUtility.TryParseHtmlString("FFDAC6", out onColor);
        light2D.color = onColor;
        light2D.intensity = 3f;
        light2D.pointLightOuterRadius = 10f;

        Invoke(nameof(TurnOff), duration);
    }

    private void TurnOff()
    {
        isDelay = false;
        rb.gravityScale = 1f;

        Color onColor;
        ColorUtility.TryParseHtmlString("FD9E6A", out onColor);
        light2D.color = onColor;
        light2D.intensity = 1.7f;
        light2D.pointLightOuterRadius = 0.9f;
        gameObject.SetActive(false);
    }
}
