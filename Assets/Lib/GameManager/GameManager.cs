using Assets.Lib.ValuePairs;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SettingsManagement;
using UnityEngine;
using UnityEngine.UI;

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

    public GameDatabase database;
    private void Awake()
    {
        database = GetComponent<GameDatabase>();
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
            int index = random.Next(debrisSprites.Length);
            var deb = Instantiate(debris, entity.transform.position, Quaternion.identity);

            var rb = deb.GetComponent<Rigidbody2D>();

            float angle = UnityEngine.Random.Range(0f, 360f);
            Vector2 direction = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            );

            rb.AddForce(direction * 1f, ForceMode2D.Impulse);

            deb.GetComponent<SpriteRenderer>().sprite = debrisSprites[index];
        }
    }


    public void SpawnAsteroidWithinArea(BoxCollider2D box)
    {

        var randomPoint = new Vector2(
            Random.Range(box.bounds.min.x, box.bounds.max.x),
            Random.Range(box.bounds.min.y, box.bounds.max.y)
        );

        var go = Instantiate(asteroid, randomPoint, Quaternion.identity);
        go.transform.parent = box.gameObject.transform;
    }


    public void SpawnShip(SpaceEntityClassId classId, bool isAI, Vector3 position, ShipSpawnSettings settings)
    {
        var v = shipClassIdValuePairs.First(s => s.key == classId);

        var go = Instantiate(v.value, position, Quaternion.identity);
        var ent = go.GetComponent<SpaceEntity>();
 

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
        }
        else
        {
            go.AddComponent<SpaceShipPlayerController>();
            database.SetPlayer(go);
        }
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
    


    public SpaceEntity FindRandomStationDestination(SpaceEntity agent, SpaceEntity currentLocation)
    {

        var stations = this.FindStationsInSector(agent);
        SpaceEntity destination;
        do
        {
            destination = stations[random.Next(stations.Length)];
        } while (destination != currentLocation);

        return destination;
    }

    private SpaceEntity[] FindStationsInSector(SpaceEntity entity)
    {
        var stations = FindObjectsByType<SpaceEntity>();
        return stations;
    }
}
