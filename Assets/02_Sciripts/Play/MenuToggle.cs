using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuToggle : MonoBehaviour
{
    public GameObject on;
    public GameObject off;

    private Toggle toggle;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
    }

    public void Toggle(bool status)
    {
        if(status) //on
        {
            on.SetActive(true);
            off.SetActive(false);
        }
        else //off
        {
            on.SetActive(false);
            off.SetActive(true);
        }
    }
}
