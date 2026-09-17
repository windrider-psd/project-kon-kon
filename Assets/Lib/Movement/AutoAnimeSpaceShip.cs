using UnityEngine;

public class AutoAnimeSpaceShip : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float rotationRate = 90f; // Degrees per second
    public float limit = 45f;        // Maximum Y rotation from starting rotation

    private float startY;
    private float currentOffset;
    private int direction = 1;

    void Start()
    {
        startY = transform.localEulerAngles.y;
    }

    void Update()
    {
        currentOffset += rotationRate * direction * Time.deltaTime;

        if (currentOffset >= limit)
        {
            currentOffset = limit;
            direction = -1;
        }
        else if (currentOffset <= -limit)
        {
            currentOffset = -limit;
            direction = 1;
        }

        transform.localRotation = Quaternion.Euler(
            transform.localEulerAngles.x,
            startY + currentOffset,
            transform.localEulerAngles.z
        );
    }
}
