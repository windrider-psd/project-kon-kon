using UnityEngine;

[CreateAssetMenu(menuName = "SpaceEntity/BaseSpaceEntity")]
public class BaseSpaceEntity : ScriptableObject
{
    public string entityClassName;

    [Header("Movement")]
    public float rotationSpeed;

    [Header("Stats")]
    public int maxHp;
    public int baseMass;


    [Header("Metadata")]
    public SpaceEntityClassId classId;
    public SpaceEntityType type;
    public GearSize gearSize;
    public int shieldSlots;
    public int cannonSlots;
    public int turrentSlots;

    [Header("Spawn Settings")]
    public Sprite sprite;
    public float objectScale;
    public float canvasScale;
    public float sliderPosition;
    



}
