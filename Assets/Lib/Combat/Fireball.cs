using UnityEngine;

public class Fireball : MonoBehaviour
{
    public int damage;
    public Quaternion direction;

    public float speed;

    public GameObject origin;

    public float duration;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, duration);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * Vector3.up * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var entity = other.GetComponent<SpaceEntity>();
        if (other.gameObject != this.origin && entity != null) {
            DoDamage(entity);
            Destroy(gameObject);
        }
    }

    void DoDamage(SpaceEntity entity)
    {
        if (entity.hp > 0) {
            entity.hp -= damage;
        }

        if (entity.hp <= 0)
        {
            Destroy(entity.gameObject);
        }
    }

}
