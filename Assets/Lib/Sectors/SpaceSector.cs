using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SpaceSector : MonoBehaviour
{
    private Dictionary<string, SpaceEntity> entities = new();

    private BoxCollider2D area;

    public SectorId sectorId;

    void Start()
    {
        area = GetComponent<BoxCollider2D>();
    }

    public void AddSpaceEntity(SpaceEntity ent)
    {
        entities.Add(ent.id, ent);
    }

    public void RemoveSpaceEntity(SpaceEntity ent) {
        entities.Remove(ent.id);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 GetRandomPointWithin()
    {
        return RandomUtils.RandomPointWithinBoxCollider(area);
    }
}
