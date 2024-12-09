using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentSpin : MonoBehaviour
{
    public float speed = 100f;

    private void Update()
    {
        transform.Rotate(0f, speed * Time.deltaTime, 0f);
    }
}
