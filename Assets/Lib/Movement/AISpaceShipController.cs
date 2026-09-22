using System;
using UnityEngine;
public class AISpaceShipController : MonoBehaviour
{
    [Header("AI Settings")]
    public float slowDownDistance = 10f;
    public float turnDeadZone = 1f;
    public float turnBeforeMovingAngle = 30f;

    [Header("Orbit Detection")]
    public float orbitDetectionDistance = 30f;
    public float orbitAngle = 70f;
    public float minimumOrbitSpeed = 2f;

    [Header("Stopping")]
    public float stopThreshold = 0.05f;

    private SpaceShipMovement movement;
    private Rigidbody2D rb;

    [SerializeField]
    private bool isStopping = true;

    private void Start()
    {
        movement = GetComponent<SpaceShipMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    public bool MoveToLocation(Transform t, float desiredDistance, bool stopAtDesiredDistance)
    {
        if (t == null)
        {
            movement.thrustInput = 0f;
            movement.rotationInput = 0f;
            return true;
        }

        Vector2 toTarget = t.position - transform.position;
        float distance = toTarget.magnitude;

        Vector2 directionToTarget = toTarget.normalized;

        float angle = Vector2.SignedAngle(
            transform.up,
            directionToTarget
        );

        // --------------------------------
        // ROTATION
        // --------------------------------

        if (angle > turnDeadZone)
        {
            movement.rotationInput = -1f;
        }
        else if (angle < -turnDeadZone)
        {
            movement.rotationInput = 1f;
        }
        else
        {
            movement.rotationInput = 0f;
        }

        // --------------------------------
        // ALREADY AT TARGET
        // --------------------------------

        if (stopAtDesiredDistance && distance <= desiredDistance)
        {
            movement.thrustInput = 0f;
            rb.linearVelocity = Vector2.zero;

            isStopping = true;

            return true;
        }

        // --------------------------------
        // DETECT ORBITING
        // --------------------------------

        Vector2 velocity = rb.linearVelocity;

        if (distance <= orbitDetectionDistance &&
            velocity.magnitude >= minimumOrbitSpeed)
        {
            float velocityAngle = Vector2.Angle(
                velocity.normalized,
                directionToTarget
            );

            // If our velocity is mostly sideways relative
            // to the target, we're likely orbiting/passing it.
            if (velocityAngle > orbitAngle)
            {
                isStopping = true;
            }
        }

        // --------------------------------
        // STOPPING STATE
        // --------------------------------

        if (isStopping)
        {
            movement.thrustInput = 0f;

            if (rb.linearVelocity.magnitude <= stopThreshold)
            {
                rb.linearVelocity = Vector2.zero;
                isStopping = false;
            }

            // IMPORTANT:
            // Don't continue into the movement code.
            return false;
        }

        // --------------------------------
        // BADLY MISALIGNED
        // --------------------------------

        if (Mathf.Abs(angle) > turnBeforeMovingAngle)
        {
            isStopping = true;
            movement.thrustInput = 0f;

            // Don't apply thrust this frame.
            return false;
        }

        // --------------------------------
        // MOVE TOWARD TARGET
        // --------------------------------

        float distanceDifference = distance - desiredDistance;


        float thrust;
        if (stopAtDesiredDistance)
        {
            thrust = distanceDifference / slowDownDistance;
        }
        else
        {
            thrust = 1;
        }

        movement.thrustInput = Mathf.Clamp01(thrust);

        return false;
    }
}