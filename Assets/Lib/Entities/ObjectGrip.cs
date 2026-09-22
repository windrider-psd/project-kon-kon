using Assets.Lib.Entities;
using Unity.InferenceEngine;
using UnityEngine;
using UnityEngine.Rendering;

public class ObjectGrip : MonoBehaviour
{
    public Transform target;

    public float gripForce = 5f;
    public float maxDistance = 10f;
    public float triggerDistance = 1f;

    public bool stopOnTrigger = true;

    private Rigidbody2D rb;
    private bool triggered = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (target == null)
            return;

        float distance = Vector2.Distance(rb.position, target.position);

        // Too far away: don't grip
        if (distance > maxDistance)
        {
            Destroy(this);
            return;
        }


        // Reached target
        if (!triggered && distance <= triggerDistance)
        {
            triggered = true;
            var entity = target.GetComponent<SpaceEntity>();
            var debris = GetComponent<SpaceDebris>();
            if(entity != null && debris != null && entity.inventory.CanAddToInventory(debris.goodsId, debris.quantity))
            {
                entity.inventory.AddToInventory(debris.goodsId, debris.quantity);
                Destroy(this.gameObject);
            }
            
        }

        if (triggered && stopOnTrigger)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // Pull towards target
        Vector2 direction = ((Vector2)target.position - rb.position).normalized;

        rb.AddForce(direction * gripForce);
    }
}