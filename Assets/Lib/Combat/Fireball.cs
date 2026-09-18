using UnityEngine;

public class Fireball : MonoBehaviour
{
    public int damage;
    public Vector3 direction;

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
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var entity = other.GetComponent<SpaceEntity>();
        if (other.gameObject != this.origin && entity != null) {
            FindAnyObjectByType<GameManager>().DoDamage(entity, damage);
            Destroy(gameObject);
        }
    }

    

}
