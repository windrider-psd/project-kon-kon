using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SpaceShipMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("Rotation")]
    public float rotationSpeed = 180f;

    private Rigidbody2D rb;

    // Input provided by the controller
    public float thrustInput;
    public float rotationInput;


    private SpaceEntity se;

    private void Awake()
    {
        this.se = GetComponent<SpaceEntity>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        var mass = se.Mass;
        var power = se.engine.power - mass;
 
        // Forward/backward movement
        if (thrustInput > 0)
        {
            rb.AddForce(
                transform.up * se.engine.acceleration * thrustInput,
                ForceMode2D.Force
            );
        }
        else if (thrustInput < 0)
        {
            rb.AddForce(
                -transform.up * se.engine.acceleration * -thrustInput * 0.5f,
                ForceMode2D.Force
            );
        }
        else
        {
            // Slow down when there is no thrust
            rb.linearVelocity = Vector2.MoveTowards(
                rb.linearVelocity,
                Vector2.zero,
                se.engine.deceleration * Time.fixedDeltaTime
            );
        }

        // Maximum speed
        if (rb.linearVelocity.magnitude > power)
        {
            rb.linearVelocity =
                rb.linearVelocity.normalized * power;
        }

        // Rotation
        rb.MoveRotation(
            rb.rotation -
            rotationInput * rotationSpeed * Time.fixedDeltaTime
        );
    }
}
