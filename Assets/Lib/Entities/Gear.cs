using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


[CreateAssetMenu(menuName = "Gear/Goods")]
public class Gear : ScriptableObject
{
    [Header("Common Attributes")]
    public GearId id;
    public string gearName;
    public GearSize gearSize;
    public int size;
    public GearSlot slot;

}

