using System;
using System.Collections.Generic;
using System.Text;


[Serializable]
public class ShipSpawnSettings
{
    public GearId engine;
    public GearId[] cannons;
    public GearId[] turrents;

    public MajorAiOrderType majorAiOrderType;
}
