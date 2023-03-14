using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class TestAgents : Agent
{
    [SerializeField] private Transform target;
    [SerializeField] private Material greyMaterial;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer floorMeshRenderer;

    private Vector3 defaultPos;

    [SerializeField] private float distance;
    [SerializeField] private float previousDistance;

    private float moveSpeed = 20f;

    public override void Initialize ()
    {
        defaultPos = transform.localPosition;

        distance = Vector3.Distance (transform.localPosition, target.transform.localPosition);
        previousDistance = 0;
    }

    public override void OnEpisodeBegin ()
    {
        //floorMeshRenderer.material = greyMaterial;

        // transform.localPosition = new Vector3 (Random.Range (-20, 20), 6, Random.Range (-20, 20));

        transform.localPosition = defaultPos;

        // target.localPosition = new Vector3(Random.Range(-20f, 20f), 2f, Random.Range(-20f, 20f));
    }

    public override void CollectObservations (VectorSensor sensor)
    {
        // sensor.AddObservation (target.localPosition);
        // sensor.AddObservation (transform.localPosition);


        sensor.AddObservation (distance);
        sensor.AddObservation (previousDistance);
        sensor.AddObservation (distance < previousDistance);

        previousDistance = distance;
    }

    public override void OnActionReceived (ActionBuffers actionBuffers)
    {
        float moveX = actionBuffers.ContinuousActions[0];
        float moveZ = actionBuffers.ContinuousActions[1];

        transform.localPosition += new Vector3 (moveX, 0, moveZ) * (Time.deltaTime * moveSpeed);

        distance = Vector3.Distance (transform.localPosition, target.transform.localPosition);

        if (distance < previousDistance)
        {
            SetReward (+0.5f);
            //EndEpisode ();
        }
        else
        {
            SetReward (-0.5f);
            // EndEpisode ();
        }
    }

    public override void Heuristic (in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;

        if (Input.GetKey (KeyCode.W))
        {
            continuousActionsOut[0] = 1;
        }
        else if (Input.GetKey (KeyCode.S))
        {
            continuousActionsOut[0] = 2;
        }

        if (Input.GetKey (KeyCode.A))
        {
            continuousActionsOut[1] = 1;
        }
        else if (Input.GetKey (KeyCode.D))
        {
            continuousActionsOut[1] = 2;
        }
    }

    private void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag ("wall"))
        {
            SetReward (-1f);
            floorMeshRenderer.material = loseMaterial;
            EndEpisode ();
        }

        if (other.CompareTag ("goal"))
        {
            SetReward (+1f);
            floorMeshRenderer.material = winMaterial;
            EndEpisode ();
        }
    }
}
