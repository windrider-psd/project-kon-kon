using Assets.Lib.Entities;
using Assets.Lib.ValuePairs;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SettingsManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;

public class GameManager : MonoBehaviour
{
    [Header("Debris")]
    public Sprite[] debrisSprites;
    public GameObject debris;
    public GameObject explosion;
    private System.Random random = new();

    [Header("Asteroid Spawn")]
    public Sprite[] asteroidSprites;
    public GameObject asteroid;

    [Header("Ship Spawn")]
    public List<ShipClassIdValuePair> shipClassIdValuePairs;

    [Header("Movement")]
    public GameObject checkpoint;


    public GameDatabase database;
    public FriendFoeManager friendFoeManager;
    private void Awake()
    {
        database = GetComponent<GameDatabase>();
        friendFoeManager = GetComponent<FriendFoeManager>();
    }

    void Start()
    {
    }

    public void DestroyEntity(SpaceEntity entity)
    {
        SpawnDebris(entity);
        Destroy(entity.gameObject);
    }

    public void DoDamage(SpaceEntity entity, float damage)
    {
        if (entity.hp > 0)
        {
            float d = damage - entity.armor.flatReduction;
            d = d / (1 + entity.armor.percentageReduction / 100);
            entity.hp -= (int)d;
        }

        if (entity.hp <= 0)
        {
            DestroyEntity(entity);
        }
    }

    private void SpawnDebris(SpaceEntity entity)
    {
        var numberOfDebris = 3;
        var exp = Instantiate(explosion, entity.transform.position, Quaternion.identity);
        for (int i = 0; i < numberOfDebris; i++)
        {
            SpawnSpaceDebris(entity, GoodsId.Scrap, 5);
        }
    }
    public void SpawnSpaceDebris(SpaceEntity origin, GoodsId goodsId, int quantity)
    {
        int index = random.Next(debrisSprites.Length);
        var deb = Instantiate(debris, origin.transform.position, Quaternion.identity);
        deb.GetComponentInChildren<SpriteRenderer>().sprite = debrisSprites[index];
        var sb = deb.GetComponent<SpaceDebris>();
        sb.quantity = quantity;
        sb.goodsId = goodsId;
    }

    public GameObject CreateCheckpoint(SpaceEntity ent, Vector3 position)
    {
        return Instantiate(checkpoint, position, Quaternion.identity);
    }

    public void SpawnAsteroidWithinArea(BoxCollider2D box)
    {

        var randomPoint = RandomUtils.RandomPointWithinBoxCollider(box);
        var ent = SpawnSpaceEntity(asteroid, randomPoint, SectorId.Ayumu);
        ent.gameObject.transform.parent = box.gameObject.transform;
    }


    public void SpawnShip(SpaceEntityClassId classId, bool isAI, Vector3 position, ShipSpawnSettings settings)
    {
        var v = shipClassIdValuePairs.First(s => s.key == classId);

        //var go = Instantiate(v.value, position, Quaternion.identity);
        //var ent = go.GetComponent<SpaceEntity>();
        var ent = SpawnSpaceEntity(v.value, position, SectorId.Ayumu);
        ent.factionId = settings.faction;
        var go = ent.gameObject;

        for(int i = 0; i < settings.turrents.Length; i++)
        {
            if(ent.turrets[i] != null)
            {
                ent.turrets[i].SetCannon(database.FindCannon(settings.turrents[i]));
            }
        }

        for (int i = 0; i < settings.cannons.Length; i++)
        {
            if (ent.cannons[i] != null)
            {
                ent.cannons[i].SetCannon(database.FindCannon(settings.cannons[i]));
            }
        }

        ent.engine = database.FindEngine(settings.engine);


        if (isAI)
        {
            go.AddComponent<AISpaceShipController>();
            var agent = go.AddComponent<AIAgent>();
            agent.SetMajorOrder(settings.majorAiOrderType);
        }
        else
        {
            go.AddComponent<SpaceShipPlayerController>();
            database.SetPlayer(go);
        }
    }

    private SpaceEntity SpawnSpaceEntity(GameObject prefab, Vector2 position, SectorId sectorId)
    {
        var go = Instantiate(prefab, position, Quaternion.identity);
        var ent = go.GetComponent<SpaceEntity>();
        ent.id = CreateSpaceEntityId();
        var sec = database.GetSector(sectorId);
        go.transform.parent = sec.transform;
        sec.AddSpaceEntity(ent);
        return ent;
    }

    public GameObject GetShipGO(SpaceEntityClassId id)
    {
        try
        {
            var v = shipClassIdValuePairs.First(s => s.key == id);
            return v.value;
        }
        catch
        {
            return null;
        }  
    }


    
   public SpaceEntity[] FindNearbyEntities(SpaceEntity origin,SpaceEntityType type, float radius)
    {
        SpaceEntity[] entities = FindObjectsByType<SpaceEntity>();

        float radiusSquared = radius * radius;
        Vector2 originPosition = origin.transform.position;

        return entities
            .Where(entity =>
                entity != origin &&
                entity.baseSpaceEntity.type == type &&
                ((Vector2)entity.transform.position - originPosition).sqrMagnitude <= radiusSquared
            )
            .ToArray();
    }


    public SpaceEntity FindRandomStationDestination(SpaceEntity agent)
    {

        var stations = this.FindStationsInSector(agent);
        SpaceEntity destination = stations[random.Next(stations.Length)];
        return destination;
    }

    public SpaceEntity FindRandomStationDestination(SpaceEntity agent, SpaceEntity currentLocation)
    {

        var stations = this.FindStationsInSector(agent);
        SpaceEntity destination;
        do
        {
            destination = stations[random.Next(stations.Length)];
        } while (destination == currentLocation);

        return destination;
    }

    private SpaceEntity[] FindStationsInSector(SpaceEntity entity)
    {
        var entities = FindObjectsByType<SpaceEntity>();
        List<SpaceEntity> list = new List<SpaceEntity>();
        foreach (SpaceEntity e in entities)
        {
            if(e.baseSpaceEntity.type == SpaceEntityType.Station)
            {
                list.Add(e); ;
            }
        }
        return list.ToArray();
    }

    private string CreateSpaceEntityId()
    {
        Guid myGuid = Guid.NewGuid();
        // 3. Convert to a string if needed
        return myGuid.ToString();
    }

    public SpaceEntity FindNearbyEnemy(SpaceEntity entity)
    {
      
    
        var nearbyShips = FindNearbyEntities(entity, SpaceEntityType.Ship, 10f);

        SpaceEntity enemy = null;
        foreach (var nearby in nearbyShips)
        {
            var rel = friendFoeManager.GetFriendliness(entity, nearby);

            if (rel == FactionFriendliness.Enemy)
            {

                enemy = nearby;
                break;
            }
        }
        return enemy;
    
    }
}
