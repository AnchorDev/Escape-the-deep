using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCup : MonoBehaviour
{
    public float floatAmplitude = 0.2f;
    public float floatSpeed  = 1f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;

        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}

