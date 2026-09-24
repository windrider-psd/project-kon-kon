using UnityEditor.Rendering;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public int damage;
    public Vector3 direction;

    public float speed;

    public GameObject origin;

    public float duration;

    public GameObject hardTarget;

    private FriendFoeManager friendFoeManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, duration);
        friendFoeManager = FindAnyObjectByType<FriendFoeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var entity = other.GetComponentInParent<SpaceEntity>();
        if(entity == null && other.transform.parent.gameObject != this.origin || origin == null)
        {
            return;
        }
        var validUniversalTarget = entity.baseSpaceEntity.type == SpaceEntityType.Debris
            || entity.baseSpaceEntity.type == SpaceEntityType.Asteroid;
        var rel = friendFoeManager.GetFriendliness(entity, this.origin.GetComponent<SpaceEntity>());

        if (
            (hardTarget == null || hardTarget == other.gameObject) 
            && (entity.baseSpaceEntity.type != SpaceEntityType.Debris) 
            && (validUniversalTarget || rel == FactionFriendliness.Enemy)
        ) {
            var ai = entity.GetComponent<AIAgent>();
            if(ai != null && entity.baseSpaceEntity.type == SpaceEntityType.Ship)
            {
                ai.Provoke(origin.GetComponent<SpaceEntity>());
            }
            FindAnyObjectByType<GameManager>().DoDamage(entity, damage);
            Destroy(gameObject);
        }
    }

    

}
