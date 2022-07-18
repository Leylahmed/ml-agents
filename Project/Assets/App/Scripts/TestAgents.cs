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

    private float moveSpeed = 15f;

    public override void OnEpisodeBegin()
    {
        //floorMeshRenderer.material = greyMaterial;

        transform.localPosition = new Vector3(Random.Range(-20f, 20f), 6f, Random.Range(-20f, 20f));

        target.localPosition = new Vector3(Random.Range(-20f, 20f), 2f, Random.Range(-20f, 20f));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(target.localPosition);
        sensor.AddObservation(transform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        float moveX = actionBuffers.ContinuousActions[0];
        float moveZ = actionBuffers.ContinuousActions[1];


        transform.localPosition += new Vector3(moveX, 0, moveZ) * (Time.deltaTime * moveSpeed);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continiousActions = actionsOut.ContinuousActions;
        continiousActions[0] = Input.GetAxisRaw("Horizontal");
        continiousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("wall"))
        {
            SetReward(-1f);
            floorMeshRenderer.material = loseMaterial;
            EndEpisode();
        }

        if (other.CompareTag("goal"))
        {
            SetReward(+2f);
            floorMeshRenderer.material = winMaterial;
            EndEpisode();
        }
    }
}
