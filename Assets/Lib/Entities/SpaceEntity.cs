using Assets.Lib.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using UnityEngine;

public class SpaceEntity : MonoBehaviour
{
    public int baseMass;
    
    public int Mass
    {
        get {
            return baseMass + armor.mass;
        }

    }
    public int maxHp;
    public int hp;
    public int shield;

    
  

    public SpaceEntityType type;


    public Engine engine;
    public Armor armor;

    public int maxCargoSpace;

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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    private void OnDestroy()
    {
        
    }

}
