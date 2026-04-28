using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Caragent : Agent
{
    public float acceleration = 20f;
    public float steeringSpeed = 120f;
    public float maxSpeed = 25f;
    public Transform startPoint;

    private Rigidbody rb;

    private void Start()
    {
        //rb = GetComponent<Rigidbody>();
        //rb.centerOfMass = new Vector3(0f, -0.5f, 0f);
    }
    public void Update()
    {
        //rb.AddForce(transform.forward * 0.1f * acceleration, ForceMode.Acceleration);
    }
    public override void Initialize()
    {
        //rb = GetComponent<Rigidbody>();
        //rb.centerOfMass = new Vector3(0f, -0.5f, 0f);
    }

    public override void OnEpisodeBegin()
    {
        //rb.linearVelocity = Vector3.zero;
        //rb.angularVelocity = Vector3.zero;

        //transform.position = startPoint.position;
        //transform.rotation = startPoint.rotation;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //Vector3 localVelocity = transform.InverseTransformDirection(rb.linearVelocity);

        //sensor.AddObservation(localVelocity.x);
        //sensor.AddObservation(localVelocity.z);
        //sensor.AddObservation(rb.angularVelocity.y);
        //sensor.AddObservation(transform.forward.x);
        //sensor.AddObservation(transform.forward.z);
        //sensor.AddObservation(transform.position.x);
        //sensor.AddObservation(transform.position.z);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        //float steer = Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f);
        //float throttle = Mathf.Clamp(actions.ContinuousActions[1], -1f, 1f);

        //Debug.Log(throttle);

        //float currentSpeed = Vector3.Dot(rb.linearVelocity, transform.forward);

        //if (currentSpeed < maxSpeed)
        //{
        //    rb.AddForce(transform.forward * throttle * acceleration, ForceMode.Acceleration);
        //}

        //if (Mathf.Abs(currentSpeed) > 0.5f)
        //{
        //    transform.Rotate(Vector3.up, steer * steeringSpeed * Time.fixedDeltaTime);
        //}

        //AddReward(0.001f);

        //if (currentSpeed > 1f)
        //{
        //    AddReward(0.001f * currentSpeed);
        //}

        //if (Vector3.Dot(transform.forward, Vector3.forward) > 0.8f)
        //{
        //    AddReward(0.002f);
        //}
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actions = actionsOut.ContinuousActions;
        actions[0] = Input.GetAxis("Horizontal");
        actions[1] = Input.GetAxis("Vertical");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            AddReward(-1f);
            EndEpisode();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            AddReward(5f);
            EndEpisode();
        }
    }
}