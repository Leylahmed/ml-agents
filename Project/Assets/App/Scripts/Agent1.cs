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

public class Agent1 : Agent
{
    [SerializeField] private float moveSpeed;

    protected override void OnEnable()
    {
        EventManager.Instance.AddListener<ShootEvent>(ShootHandler);
        EventManager.Instance.AddListener<ShootEvent2>(Shoot2Handler);
    }

    protected override void OnDisable()
    {
        EventManager.Instance.RemoveListener<ShootEvent>(ShootHandler);
        EventManager.Instance.RemoveListener<ShootEvent2>(Shoot2Handler);
    }

    public override void OnEpisodeBegin()
    {
        transform.position = new Vector3(Random.Range(-5f, 5f), 1f, Random.Range(-5f, 5f));
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

    private void ShootHandler(ShootEvent eventDetails)
    {
        SetReward(+1.5f);
        EndEpisode();
    }

    private void Shoot2Handler(ShootEvent2 eventDetails)
    {
        SetReward(-1.5f);
        EndEpisode();
    }
}
