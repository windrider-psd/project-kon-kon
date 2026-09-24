using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.EventSystems.EventTrigger;

public class CameraSpaceEntityRaycast : MonoBehaviour
{

    public event Action<SpaceEntity> onSpaceEntitySelected;

    public Camera mainCamera;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Vector3 screenPosition = new Vector3(
            mousePosition.x,
            mousePosition.y,
            -mainCamera.transform.position.z
        );

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);
  
        RaycastHit2D[] hits = Physics2D.RaycastAll(worldPosition, Vector2.zero);

        SpaceEntity hit = null;

        foreach(var h in hits)
        {       
            var entity = h.collider.GetComponentInParent<SpaceEntity>();
            if (entity != null)
            {
                hit = entity; break;
            }
        }


        
        onSpaceEntitySelected?.Invoke(hit);
    }
}
