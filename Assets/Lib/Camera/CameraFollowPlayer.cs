using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothSpeed = 5f;

    [Header("Zoom")]
    public float zoomSpeed = 5f;
    public float minZoom = 20f;
    public float maxZoom = 80f;

    private Camera cam;

    private GameDatabase database;
    void Start()
    {
        database = FindAnyObjectByType<GameDatabase>();
        database.onPlayerChanged += this.ChangePlayer;
        cam = GetComponent<Camera>();
    }

    private void ChangePlayer()
    {
        this.target = database.GetPlayer().transform;
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        // Follow target
        Vector3 desiredPosition = target.position + offset;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );

        // Mouse wheel zoom
        if (Mouse.current != null)
        {
            float scroll = Mouse.current.scroll.ReadValue().y;

            cam.fieldOfView -= scroll * zoomSpeed * Time.deltaTime;

            // Limit zoom
            cam.fieldOfView = Mathf.Clamp(
                cam.fieldOfView,
                minZoom,
                maxZoom
            );
        }
    }
    }
