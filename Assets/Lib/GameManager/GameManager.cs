using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [Header("Debris")]
    public Sprite[] debrisSprites;
    public GameObject debris;
    public GameObject explosion;
    private System.Random random = new();

    [Header("Asteroids")]
    public Sprite[] asteroidSprites;
    public GameObject asteroid;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
        Destroy(exp, 3f);
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
}
