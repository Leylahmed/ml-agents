using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DynamicBox.EventManagement;

public class AgentController : MonoBehaviour
{
    private void OnEnable ()
    {
        EventManager.Instance.AddListener<ShootEvent> (ShootHandler);
    }

    private void OnDisable ()
    {
        EventManager.Instance.RemoveListener<ShootEvent> (ShootHandler);
    }

    private void ShootHandler (ShootEvent eventDetails)
    {
    }
}
