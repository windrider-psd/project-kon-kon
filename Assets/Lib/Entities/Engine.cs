

using UnityEngine;
[CreateAssetMenu(menuName = "Gear/Engine")]
public class Engine : Gear
{
    [Header("Engine Attributes")]
    public int power;
    public int acceleration;
    public int deceleration;
}

