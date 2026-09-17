using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SpaceShipMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Movement")]
    public float acceleration = 5f;
    public float maxSpeed = 10f;
    public float deceleration = 2f;

    [Header("Rotation")]
    public float rotationSpeed = 180f;

    private Rigidbody2D rb;

    // Input provided by the controller
    public float thrustInput;
    public float rotationInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Forward/backward movement
        if (thrustInput > 0)
        {
            rb.AddForce(
                transform.up * acceleration * thrustInput,
                ForceMode2D.Force
            );
        }
        else if (thrustInput < 0)
        {
            rb.AddForce(
                -transform.up * acceleration * -thrustInput * 0.5f,
                ForceMode2D.Force
            );
        }
        else
        {
            // Slow down when there is no thrust
            rb.linearVelocity = Vector2.MoveTowards(
                rb.linearVelocity,
                Vector2.zero,
                deceleration * Time.fixedDeltaTime
            );
        }

        // Maximum speed
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity =
                rb.linearVelocity.normalized * maxSpeed;
        }

        // Rotation
        rb.MoveRotation(
            rb.rotation -
            rotationInput * rotationSpeed * Time.fixedDeltaTime
        );
    }
}
