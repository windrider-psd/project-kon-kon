using UnityEngine;
using UnityEngine.InputSystem;

public class SpaceShipPlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private SpaceShipMovement movement;
    private SpaceShipCannon[] cannons;
    private void Awake()
    {
        movement = GetComponent<SpaceShipMovement>();
        cannons = GetComponentsInChildren<SpaceShipCannon>();
    }

    private void Update()
    {
        // Thrust
        if (Keyboard.current.wKey.isPressed)
        {
            movement.thrustInput = 1f;
        }
        else if (Keyboard.current.sKey.isPressed)
        {
            movement.thrustInput = -1f;
        }
        else
        {
            movement.thrustInput = 0f;
        }

        // Rotation
        if (Keyboard.current.aKey.isPressed)
        {
            movement.rotationInput = -1f;
        }
        else if (Keyboard.current.dKey.isPressed)
        {
            movement.rotationInput = 1f;
        }
        else
        {
            movement.rotationInput = 0f;
        }

        if (Keyboard.current.spaceKey.isPressed)
        {
            foreach (var space in cannons)
            {
                space.Fire();
            }
        }
    }
}
