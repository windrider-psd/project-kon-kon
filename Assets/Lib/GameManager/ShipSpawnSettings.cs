using System;
using System.Collections.Generic;
using System.Text;


[Serializable]
public class ShipSpawnSettings
{
    public EngineClassId engine;
    public SpaceCannonClassId[] cannons;
    public SpaceCannonClassId[] turrents;
}
