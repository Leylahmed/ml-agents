using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using DynamicBox.EventManagement;

public class Agent2 : Agent
{
    [SerializeField] private float moveSpeed;

    public override void OnEpisodeBegin()
    {
        transform.position = new Vector3(Random.Range(-20f, 20f), 0f, Random.Range(-20f, 20f));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        float moveX = actionBuffers.ContinuousActions[0];
        float moveZ = actionBuffers.ContinuousActions[1];

        transform.position += new Vector3(moveX, 0, moveZ) * (Time.deltaTime * moveSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("wall") || other.CompareTag("zone"))
        {
            SetReward(-0.5f);
            EndEpisode();
        }


    }
}
