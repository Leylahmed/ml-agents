using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class CustomAgent : Agent
{
    [Header("Links")]
    [SerializeField] private Rigidbody rigid;

    private Vector3 defaultPosition;
    private Quaternion defaultRotation;

    public override void Initialize ()
    {
        defaultPosition = transform.position;
        defaultRotation = transform.rotation;
    }

    public override void OnEpisodeBegin ()
    {
        transform.position = defaultPosition;
        transform.rotation = defaultRotation;
    }

    public override void OnActionReceived (ActionBuffers actionBuffers)
    {
        float forward = actionBuffers.ContinuousActions[0];
        Vector3 rotation = transform.up * -actionBuffers.ContinuousActions[1];

        transform.Rotate (rotation, Time.deltaTime * 100f);
        //transform.position = Vector3.MoveTowards (transform.position, transform.position + forward * transform.forward, 0.1f);
        rigid.AddForce(transform.forward * forward,
            ForceMode.VelocityChange);

        Shoot ();
    }

    public override void CollectObservations (VectorSensor sensor)
    {
    }

    public override void Heuristic (in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.ContinuousActions;

        if (Input.GetKey (KeyCode.W))
        {
            discreteActionsOut[0] = 1;
        }
        else if (Input.GetKey (KeyCode.S))
        {
            discreteActionsOut[0] = -1;
        }
        else if (Input.GetKey (KeyCode.A))
        {
            discreteActionsOut[1] = 1;
        }
        else if (Input.GetKey (KeyCode.D))
        {
            discreteActionsOut[1] = -1;
        }
    }

    public void Shoot ()
    {
        if (Physics.Raycast (new Vector3 (transform.position.x, 0.5f, transform.position.z), transform.forward, out var hit, 4f))
        {
            if (hit.collider.CompareTag ("agent"))
            {
                Debug.Log ("Success");
                SetReward (1);
                hit.transform.GetComponent <CustomAgent>().EndEpisode ();
                EndEpisode ();
            }
        }
    }
}
