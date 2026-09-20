

using UnityEngine;
[CreateAssetMenu(menuName = "Gear/Engine")]
public class Engine : ScriptableObject
{
    public string engineName;
    public int power;
    public int acceleration;

    public int deceleration;

    public int size;

    public GearSize gearSize;

    public EngineClassId id;
}

