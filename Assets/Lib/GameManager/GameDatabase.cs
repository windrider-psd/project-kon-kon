using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameDatabase : MonoBehaviour
{

    private Engine[] engines;

    private List<Tuple<GearId, Engine>> enginesMap = new();

    private SpaceCannon[] cannons;
    private List<Tuple<GearId, SpaceCannon>> cannonsMap = new();

    private GameObject player;

    public event Action onPlayerChanged;



    void Awake()
    {
        LoadEngines();
        LoadWeapons();
    }
    
    public SpaceCannon FindCannon(GearId id)
    {
        return cannons.First(e => e.id == id);
    }

    public Engine FindEngine(GearId id)
    {
        return engines.First(e => e.id == id);
    }
    
    private void LoadEngines()
    {
        var ent = Resources.LoadAll<Engine>("Data/Gear/Engines");
        foreach (var e in ent)
        {
            enginesMap.Add(new(e.id, e));
            
        }
        engines = ent;
    }

    private void LoadWeapons()
    {
        var ent = Resources.LoadAll<SpaceCannon>("Data/Gear/Cannons");

        foreach (var e in ent)
        {
            cannonsMap.Add(new(e.id, e));
        }
        cannons = ent;
    }

    public GameObject SetPlayer(GameObject player)
    {
        
        this.player = player;
        onPlayerChanged?.Invoke();
        return player;
    }

    public GameObject GetPlayer()
    {
        return player;
    }
}
