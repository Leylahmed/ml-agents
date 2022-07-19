using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    [SerializeField] private Transform plane1;
    [SerializeField] private Transform plane2;
    [SerializeField] private Transform plane3;
    [SerializeField] private Transform plane4;

    [SerializeField] private float zoneSpeed;

    private void Update()
    {
        plane1.localScale = new Vector3(plane1.localScale.x + zoneSpeed * Time.deltaTime, plane1.localScale.y,
            plane1.localScale.z);

        plane2.localScale = new Vector3(plane2.localScale.x + zoneSpeed * Time.deltaTime, plane2.localScale.y,
            plane2.localScale.z);

        plane3.localScale = new Vector3(plane3.localScale.x, plane3.localScale.y,
            plane3.localScale.z + zoneSpeed * Time.deltaTime);

        plane4.localScale = new Vector3(plane4.localScale.x, plane4.localScale.y,
            plane4.localScale.z + zoneSpeed * Time.deltaTime);
    }
}
