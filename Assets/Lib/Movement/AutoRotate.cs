using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    public float rotationRate = 90f; // Degrees per second

    void Update()
    {
        transform.Rotate(0f, 0f, rotationRate * Time.deltaTime);
    }
}
