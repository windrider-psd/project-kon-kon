using Assets.Lib.ValuePairs;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using UnityEngine;


public class SpaceEntity : MonoBehaviour
{

    public BaseSpaceEntity baseSpaceEntity;

    public int Mass
    {
        get {
            return baseSpaceEntity.baseMass + armor.mass;
        }

    }

    public int hp;

    public Engine engine;
    public Armor armor;

    public SpaceShipCannon[] cannons;

    public SpaceShipTurret[] turrets;

    public int maxCargoSpace;

    public void Start()
    {
        hp = baseSpaceEntity.maxHp;
    }

    public int CargoSpace {  
        get
        {
            int val = engine.size;
            foreach(ValuePair thing in inventory)
            {
                val += thing.value;
            }
            return val;
        } 
    }


    public ValuePair[] inventory;

}
