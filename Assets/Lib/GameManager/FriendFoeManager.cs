using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class FriendFoeManager : MonoBehaviour
{

    public FactionRelationEntry[] factionRelations;
    private readonly Dictionary<(string, FactionId), float> actorFactionRelations = new();

    private readonly Dictionary<(string, string), float> actorRelations = new();

    void Awake()
    {
        
        /*
        foreach(var entry in startingRelation)
        {
            factionRelations.Add((entry.faction1, entry.faction2), entry.value);
            factionRelations.Add((entry.faction2, entry.faction1), entry.value);
        }
        */

    }
    public FactionFriendliness GetFriendliness(SpaceEntity a, SpaceEntity b)
    {
        var val = GetRelation(a, b);

        if (val >= 80f)
        {
            return FactionFriendliness.Ally;
        }
        else if (val >= 40f)
        {
            return FactionFriendliness.Friendly;
        }
        if (val >= -20f)
        {
            return FactionFriendliness.Neutral;
        }
        if (val >= -50f)
        {
            return FactionFriendliness.Hostile;
        }
        else
        {
            return FactionFriendliness.Enemy;
        }
    }
    public float GetRelation(SpaceEntity a, SpaceEntity b)
    {

        if(a == b)
        {
            return 100f;
        }

        if(a.factionId == b.factionId && a.factionId != FactionId.None)
        {
            return 100f;
        }
        else if(a.factionId == FactionId.None && b.factionId == FactionId.None)
        {
            return GetRelationBettwenActors(a, b);
        }
        else if(a.factionId != FactionId.None && b.factionId == FactionId.None)
        {
            return GetRelationBettwenFactionAndActor(a, b);
        }

        else if (a.factionId == FactionId.None && b.factionId != FactionId.None)
        {
            return GetRelationBettwenFactionAndActor(b, a);
        }
        else
        {
            return GetRelationBettwenFactions(a, b);
        }

    }

    private float GetRelationBettwenFactions(SpaceEntity a, SpaceEntity b)
    {
        foreach(var rel in factionRelations)
        {
            if(
                (a.factionId == rel.faction1 || a.factionId == rel.faction2) 
                && (b.factionId == rel.faction1 || b.factionId == rel.faction2))
            {
                return rel.value;
            }
        }
        return 0f;
    }
    private float GetRelationBettwenFactionAndActor(SpaceEntity f, SpaceEntity a)
    {
        if(actorFactionRelations.ContainsKey((a.id, f.factionId)))
        {
            return actorFactionRelations[(a.id, a.factionId)];
        }
        else
        {
            return 0f;
        }
    }

    private float GetRelationBettwenActors(SpaceEntity a, SpaceEntity b)
    {
        if(actorRelations.ContainsKey((a.id, b.id)))
        {
            return actorRelations[(a.id, b.id)];
        }
        else if (actorRelations.ContainsKey((b.id, a.id)))
        {
            return actorRelations[(b.id, a.id)];
        }
        return 0f;
    }

        
    


    private (FactionId, FactionId) GetKey(FactionId a, FactionId b)
    {
        if (a < b)
            return (a, b);

        return (b, a);
    }
}


public enum FactionFriendliness
{
    Enemy, Hostile, Neutral, Friendly, Ally
}