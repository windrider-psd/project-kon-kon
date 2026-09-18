using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Debris")]
    public Sprite[] debrisSprites;
    public GameObject debris;
    public GameObject explosion;
    private System.Random random = new();

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
        Instantiate(explosion, entity.transform.position, Quaternion.identity);

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
}
