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

    public int maxCargoSpace;

    public void Start()
    {
        manager = FindAnyObjectByType<GameManager>();
        if (baseSpaceEntity == null)
        {
            hp = 1;
            return;
        }
        hp = baseSpaceEntity.maxHp;
    }

    public int CargoSpace {  
        get
        {
            int val = engine.size;
            foreach(InventoryEntry thing in inventory)
            {
                val += thing.value;
            }
            return val;
        } 
    }

    public void GripNearbyScrape()
    {
        var nearby = manager.FindNearbyEntities(this, SpaceEntityType.Debris, 10f);
        Debug.Log(nearby.Length);
        foreach(SpaceEntity n in nearby)
        {
            var grip = n.GetOrAddComponent<ObjectGrip>();
            if(grip.target ==  null)
            grip.target = this.transform;
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


    public void AddToInventory(GoodsId goodsId, int quantity)
    {
        if (quantity <= 0)
            return;

        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i].key == goodsId)
            {
                inventory[i].value += quantity;
                return;
            }
        }

        Array.Resize(ref inventory, inventory.Length + 1);

        inventory[inventory.Length - 1] = new InventoryEntry
        {
            key = goodsId,
            value = quantity
        };
    }
    public InventoryEntry[] inventory;

}
