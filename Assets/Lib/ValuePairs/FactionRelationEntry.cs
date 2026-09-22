using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


[Serializable]
public class FactionRelationEntry
{
    public FactionId faction1;
    public FactionId faction2;

    [Range(-100f, 100f)]
    public float value;
}

