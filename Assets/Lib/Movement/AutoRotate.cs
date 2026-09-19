using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    public float rotationRate = 90f; // Degrees per second
    public float randomOffset;


    private float trueRate;
    public void Start()
    {
        if(randomOffset != 0)
        {
            var offset = UnityEngine.Random.Range(-randomOffset, randomOffset);
            trueRate = rotationRate + offset;
        }
        else
        {
            trueRate = rotationRate;
        }
    }
    void Update()
    {
        transform.Rotate(0f, 0f, trueRate * Time.deltaTime);
    }
}
