using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DynamicBox.EventManagement;

public class AgentController : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;

    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 100f,layerMask))
        {
            if (hit.collider.CompareTag("agent"))
            {
                EventManager.Instance.Raise(new ShootEvent());
            }

            if (hit.collider.CompareTag("agent2"))
            {
                EventManager.Instance.Raise(new ShootEvent2());
            }
        }
    }
}
