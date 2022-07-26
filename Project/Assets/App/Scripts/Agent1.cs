using System;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Random = UnityEngine.Random;

public enum Teams
{
    Xbot = 0,
    Ybot = 1
}

public class Agent1 : Agent, IAgent1Interface
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;

    [SerializeField] private LayerMask layerMask;

    private Teams team;

    private int opponentID;

    private string opponentTag;

    private bool shoot;

    private bool alive;

    private RaycastHit hit;

    BehaviorParameters m_BehaviorParameters;


    public override void Initialize ()
    {
        m_BehaviorParameters = gameObject.GetComponent<BehaviorParameters> ();

        alive = true;

        team = m_BehaviorParameters.TeamId == (int)Teams.Xbot ? Teams.Xbot : Teams.Ybot;

        if (team == Teams.Xbot)
        {
            opponentTag = "agent2";
            opponentID = 1;
        }
        else
        {
            opponentTag = "agent";
            opponentID = 0;
        }
    }

    private void Update ()
    {
        if (alive)
        {
            SetReward (+0.7f);
            // AddGroupReward (+1);
        }
        else
        {
            SetReward (-1.5f);

            //gameObject.SetActive (false);

            EndEpisode ();
        }
    }

    public override void OnEpisodeBegin ()
    {
        shoot = false;

        transform.position = new Vector3 (Random.Range (-20f, 20f), 0f, Random.Range (-20f, 20f));
    }

    public override void CollectObservations (VectorSensor sensor)
    {
        sensor.AddObservation ((transform.position.x) / 20f);
        sensor.AddObservation ((transform.position.z) / 20f);
        sensor.AddObservation ((transform.rotation.y) / 360f);

        sensor.AddObservation (shoot);
    }

    public override void OnActionReceived (ActionBuffers actionBuffers)
    {
        ActionSegment<float> continuousActions = actionBuffers.ContinuousActions;
        ActionSegment<int> discreteActions = actionBuffers.DiscreteActions;

        float moveX = continuousActions[0];
        float moveZ = continuousActions[1];
        float rotateY = continuousActions[2];

        transform.position += new Vector3 (moveX, 0, moveZ) * (Time.deltaTime * moveSpeed);
        transform.Rotate (Vector3.right, rotateY * rotateSpeed * Time.deltaTime);

        if (team == Teams.Xbot)
        {
            Shoot (Teams.Xbot);
        }
        else
        {
            Shoot (Teams.Ybot);
        }
    }

    private void Shoot (Teams team)
    {
        if (Physics.Raycast (transform.position, transform.forward, out hit, 100f, layerMask))
        {
            if (hit.collider.CompareTag (opponentTag))
            {
                shoot = true;
                hit.collider.GetComponent<IAgent1Interface> ().GetDamage (opponentID);
                SetReward (+1.5f);
            }
            else
            {
                shoot = false;
            }
        }
    }

    public void GetDamage (int id)
    {
        if (id == 0)
        {
            Debug.Log ("died id= 0");
        }

        if (id == 1)
        {
            Debug.Log ("died id= 1");
        }

        alive = false;
    }


    private void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag ("wall") || other.CompareTag ("zone"))
        {
            SetReward (-0.5f);
        }
    }

    private void OnTriggerExit (Collider other)
    {
        if (other.CompareTag ("zone"))
        {
            SetReward (+0.5f);
        }
    }
}
