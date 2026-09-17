using UnityEngine;

public class StardustFollowCamera : MonoBehaviour
{
    public Transform cameraTransform;

    [Header("Movement")]
    public float rotationSpeed = 2f;
    public float movementSpeed = 0.5f;

    private Vector3 lastCameraPosition;
    private Quaternion lastCameraRotation;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        lastCameraPosition = cameraTransform.position;
        lastCameraRotation = cameraTransform.rotation;
    }

    void LateUpdate()
    {
        // Follow the camera's position
        transform.position = cameraTransform.position;

        // Calculate how much the camera rotated
        Quaternion rotationDelta =
            cameraTransform.rotation * Quaternion.Inverse(lastCameraRotation);

        // Apply a portion of that rotation to the stardust
        transform.rotation =
            Quaternion.Slerp(
                transform.rotation,
                rotationDelta * transform.rotation,
                rotationSpeed * Time.deltaTime
            );

        lastCameraPosition = cameraTransform.position;
        lastCameraRotation = cameraTransform.rotation;
    }
}