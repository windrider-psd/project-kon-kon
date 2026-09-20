using System;
using UnityEngine;

public class AISpaceShipController : MonoBehaviour
{
    public Transform target;

    [Header("AI Settings")]
    public float desiredDistance = 5f;
    public float slowDownDistance = 10f;
    public float turnDeadZone = 5f;
    public float turnBeforeMovingAngle = 30f;

    [Header("Stopping")]
    public float stopThreshold = 0.05f;

    private SpaceShipMovement movement;
    private Rigidbody2D rb;

    private bool isStopping = true;

    [SerializeField]
    public AISpaceShipOrder currentOrder;


    public Action onOrderConcluded;

    private void Awake()
    {
        movement = GetComponent<SpaceShipMovement>();
        rb = GetComponent<Rigidbody2D>();
    }


    private void Move()
    {
        if (target == null)
        {
            if(currentOrder != AISpaceShipOrder.Idle)
            {
                onOrderConcluded?.Invoke();
            }
            movement.thrustInput = 0f;
            movement.rotationInput = 0f;
            currentOrder = AISpaceShipOrder.Idle;
            return;
        }

        Vector2 toTarget = target.position - transform.position;
        float distance = toTarget.magnitude;

        Vector2 directionToTarget = toTarget.normalized;

        float angle = Vector2.SignedAngle(
            transform.up,
            directionToTarget
        );

        // Rotate toward target
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
            if (currentOrder == AISpaceShipOrder.Kill)
            {
                var comps = GetComponentsInChildren<SpaceShipCannon>();
                foreach (SpaceShipCannon comp in comps)
                {
                    comp.Fire();
                }
            }
        }

        // Already close enough
        if (distance <= desiredDistance)
        {
            movement.thrustInput = 0f;
            rb.linearVelocity = Vector2.zero;
            if (currentOrder == AISpaceShipOrder.Move) {
                onOrderConcluded?.Invoke();
                SetOrder(AISpaceShipOrder.Idle);
                return;
            }
            isStopping = true;
            return;
        }

        // STOPPING STATE
        if (isStopping)
        {
            movement.thrustInput = 0f;

            // Wait until the ship has completely stopped
            if (rb.linearVelocity.magnitude <= stopThreshold)
            {
                rb.linearVelocity = Vector2.zero;
                isStopping = false;
            }

            return;
        }

        // If we're badly misaligned, start stopping again
        if (Mathf.Abs(angle) > turnBeforeMovingAngle)
        {
            isStopping = true;
            movement.thrustInput = 0f;
            return;
        }

        // We're stopped and facing the target.
        // Now we can move.
        float distanceDifference = distance - desiredDistance;

        float thrust = distanceDifference / slowDownDistance;

        movement.thrustInput = Mathf.Clamp01(thrust);
    }

    private void Update()
    {
        if(currentOrder != AISpaceShipOrder.Idle) {
            Move();
        }
    }

    public void SetOrder(AISpaceShipOrder order)
    {
        this.target = null;
        currentOrder = order;
        isStopping = false;
    }

    public void SetOrder(AISpaceShipOrder order, Transform target)
    {
        this.currentOrder = order;
        this.target = target;
        isStopping = false;
    }
}
