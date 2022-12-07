using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonRotation : MonoBehaviour
{
    public float xAngle, yAngle, zAngle;

    void Update()
    {
        gameObject.transform.Rotate(xAngle, yAngle, zAngle, Space.Self);
    }
}
