using UnityEditor;
using UnityEngine;


public class ThrowRandomDirection : MonoBehaviour
{

    public float force = 5f;
    public float drag = 2f;
    public float stopThreshold = 0.05f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearDamping = drag;

        Throw();
    }

    private void FixedUpdate()
    {
        if (rb.linearVelocity.magnitude < stopThreshold)
        {
            rb.linearVelocity = Vector2.zero;
            Destroy(this);
        }
    }

    private void Throw()
    {
        float angle = Random.Range(0f, 360f);

        Vector2 direction = new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad)
        );

        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }
}
