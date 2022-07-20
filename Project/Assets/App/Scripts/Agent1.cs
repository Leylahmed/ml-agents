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
    #region UI

    #endregion


    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;

    [SerializeField] private LayerMask layerMask;

    private Teams team;

    private bool shoot;

    private bool alive;

    private RaycastHit hit;

    BehaviorParameters m_BehaviorParameters;


    public override void Initialize ()
    {
        m_BehaviorParameters = gameObject.GetComponent<BehaviorParameters> ();

        team = m_BehaviorParameters.TeamId == (int)Teams.Xbot ? Teams.Xbot : Teams.Ybot;
    }

    public override void OnEpisodeBegin ()
    {
        alive = true;

        shoot = false;

        transform.position = new Vector3 (Random.Range (-20f, 20f), 0f, Random.Range (-20f, 20f));
    }

    public override void CollectObservations (VectorSensor sensor)
    {
        sensor.AddObservation (transform.position);
        sensor.AddObservation (transform.rotation);
        sensor.AddObservation (shoot);
        sensor.AddObservation (alive);
    }

    public override void OnActionReceived (ActionBuffers actionBuffers)
    {
        float moveX = actionBuffers.ContinuousActions[0];
        float moveZ = actionBuffers.ContinuousActions[1];
        float rotate = actionBuffers.ContinuousActions[2];

        transform.position += new Vector3 (moveX, 0, moveZ) * (Time.deltaTime * moveSpeed);
        transform.Rotate (Vector3.right, rotate * rotateSpeed * Time.deltaTime);

        if (team == Teams.Xbot)
        {
            Shoot (Teams.Xbot);
        }
        else
        {
            Shoot (Teams.Ybot);
        }

        if (alive)
        {
            SetReward (0.7f);
            EndEpisode ();
        }
        else
        {
            gameObject.SetActive (false);
        }
    }

    private void Shoot (Teams team)
    {
        string tag;
        int opponentID;

        if (team == Teams.Xbot)
        {
            tag = "agent2";
            opponentID = 1;
        }
        else
        {
            tag = "agent";
            opponentID = 0;
        }

        if (Physics.Raycast (transform.position, transform.forward, out hit, 100f, layerMask))
        {
            if (hit.collider.CompareTag (tag))
            {
                shoot = true;
                hit.collider.GetComponent<IAgent1Interface> ().GetDamage (opponentID);
                SetReward (+1.5f);
                EndEpisode ();
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
            alive = false;
            SetReward (-1.5f);
            EndEpisode ();
        }

        if (id == 1)
        {
            alive = false;
            SetReward (-1.5f);
            EndEpisode ();
        }
    }


    private void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag ("wall") || other.CompareTag ("zone"))
        {
            SetReward (+0.5f);
            EndEpisode ();
        }
    }

    private void OnTriggerExit (Collider other)
    {
        if (other.CompareTag ("zone"))
        {
            SetReward (-0.5f);
            EndEpisode ();
        }
    }
}
