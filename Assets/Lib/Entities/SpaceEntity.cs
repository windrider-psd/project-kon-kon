using Assets.Lib.ValuePairs;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using Unity.VisualScripting;
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

    public GameManager manager;

    public string id;

    public Inventory inventory;

    public void Start()
    {
        
        manager = FindAnyObjectByType<GameManager>();
        if (baseSpaceEntity == null)
        {
            hp = 1;
            return;
        }
        hp = baseSpaceEntity.maxHp;
        inventory.maxCargoSpace = baseSpaceEntity.maxCargoSpace;
    }

    public void GripNearbyScrape()
    {
        var nearby = manager.FindNearbyEntities(this, SpaceEntityType.Debris, 10f);
        foreach(SpaceEntity n in nearby)
        {
            var grip = n.GetOrAddComponent<ObjectGrip>();
            if(grip.target ==  null)
            grip.target = this.transform;
        }
    }

    public void ShootCannonsInRange(Transform target)
    {
        var comps = GetComponentsInChildren<SpaceShipCannon>();
        foreach (SpaceShipCannon comp in comps)
        {
            if (comp.IsInRange(target))
            {
                comp.Fire();
            }
            
        }
    }

    public void ShootCannons()
    {
        var comps = GetComponentsInChildren<SpaceShipCannon>();
        foreach (SpaceShipCannon comp in comps)
        {
            
            comp.Fire();
        }


    }


    

}
