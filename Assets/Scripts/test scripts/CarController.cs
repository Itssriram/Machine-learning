using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    public float acceleration = 20f;
    public float reverseAcceleration = 10f;
    public float maxForwardSpeed = 25f;
    public float maxReverseSpeed = 10f;
    public float steeringForce = 120f;
    public float downforce = 50f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rb.centerOfMass = new Vector3(0f, -0.5f, 0f);
    }

    private void FixedUpdate()
    {
        float moveInput = Input.GetAxisRaw("Vertical");
        float steerInput = Input.GetAxisRaw("Horizontal");

        float currentSpeed = Vector3.Dot(rb.linearVelocity, transform.forward);

        rb.AddForce(-transform.up * downforce, ForceMode.Acceleration);

        if (moveInput > 0f && currentSpeed < maxForwardSpeed)
        {
            rb.AddForce(transform.forward * acceleration * moveInput, ForceMode.Acceleration);
        }
        else if (moveInput < 0f && currentSpeed > -maxReverseSpeed)
        {
            rb.AddForce(transform.forward * reverseAcceleration * moveInput, ForceMode.Acceleration);
        }

        if (Mathf.Abs(currentSpeed) > 0.5f)
        {
            float turnAmount = steerInput * steeringForce * Time.fixedDeltaTime;
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, turnAmount, 0f));
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetCar();
        }
    }

    private void ResetCar()
    {
        rb.position = new Vector3(0f, 1f, 0f);
        rb.rotation = Quaternion.identity;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}