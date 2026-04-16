using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class CarAgent : Agent
{
    [Header("References")]
    public Rigidbody rb;
    public Transform[] checkpoints;

    [Header("Movement")]
    public float acceleration = 8f;
    public float steering = 180f;
    public float maxSpeed = 10f;

    [Header("Training")]
    public float stuckThreshold = 0.2f;

    private int currentCheckpoint = 0;
    private Vector3 startPos;
    private Quaternion startRot;

    public override void Initialize()
    {
        //startPos = transform.position;
        //startRot = transform.rotation;
    }

    public override void OnEpisodeBegin()
    {
        // Reset position
        transform.position = startPos;
        transform.rotation = startRot;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        currentCheckpoint = 0;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Speed
        sensor.AddObservation(rb.linearVelocity.magnitude / maxSpeed);

        // Direction to next checkpoint (LOCAL SPACE)
        Vector3 dir = (checkpoints[currentCheckpoint].position - transform.position).normalized;
        Vector3 localDir = transform.InverseTransformDirection(dir);
        sensor.AddObservation(localDir);

        // Alignment (dot product)
        float alignment = Vector3.Dot(transform.forward, dir);
        sensor.AddObservation(alignment);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float move = Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f);
        float turn = Mathf.Clamp(actions.ContinuousActions[1], -1f, 1f);

        // Apply movement
        rb.AddForce(transform.forward * move * acceleration, ForceMode.Acceleration);
        transform.Rotate(Vector3.up, turn * steering * Time.deltaTime);

        // Clamp speed
        rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxSpeed);

        // Reward for moving toward checkpoint
        Vector3 toCheckpoint = (checkpoints[currentCheckpoint].position - transform.position).normalized;
        float alignment = Vector3.Dot(transform.forward, toCheckpoint);
        AddReward(alignment * 0.01f);

        // Small time penalty
        AddReward(-0.001f);

        // Penalize being stuck
        if (rb.linearVelocity.magnitude < stuckThreshold)
        {
            AddReward(-0.01f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Checkpoint logic
        if (other.CompareTag("Checkpoint"))
        {
            if (other.transform == checkpoints[currentCheckpoint])
            {
                AddReward(1.0f);
                currentCheckpoint++;

                if (currentCheckpoint >= checkpoints.Length)
                {
                    AddReward(5.0f); // Lap complete
                    EndEpisode();
                }
            }
        }

        // Collision penalties
        if (other.CompareTag("Wall"))
        {
            AddReward(-1.0f);
            EndEpisode();
        }

        if (other.CompareTag("OffTrack"))
        {
            AddReward(-2.0f);
            EndEpisode();
        }
    }

    // Optional: manual testing
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actions = actionsOut.ContinuousActions;
        actions[0] = Input.GetAxis("Vertical");   // W/S
        actions[1] = Input.GetAxis("Horizontal"); // A/D
    }
}