using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    [SerializeField] private float zoneSpeed;

    private void Update()
    {
        var transform1 = transform;
        var localScale = transform1.localScale;
        localScale = new Vector3(localScale.x - zoneSpeed * Time.deltaTime, localScale.y,
            localScale.z - zoneSpeed * Time.deltaTime);
        transform1.localScale = localScale;
    }
}
