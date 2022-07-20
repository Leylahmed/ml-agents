using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public enum Teams
{
    Xbot = 0,
    Ybot = 1
}

public class Agent1 : Agent
{
    [SerializeField] private float moveSpeed;

    [SerializeField] private LayerMask layerMask;

    public Teams team;

    BehaviorParameters m_BehaviorParameters;



    public override void Initialize()
    {
        m_BehaviorParameters = gameObject.GetComponent<BehaviorParameters>();

        team = m_BehaviorParameters.TeamId == (int)Teams.Xbot ? Teams.Xbot : Teams.Ybot;
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

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 100f,layerMask))
        {
            if (hit.collider.CompareTag("agent"))
            {
                SetReward(+1.5f);
                EndEpisode();
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("wall") || other.CompareTag("zone"))
        {
            SetReward(-0.5f);
            EndEpisode();
        }
        else
        {
            SetReward(+0.5f);
            EndEpisode();
        }
    }
}
