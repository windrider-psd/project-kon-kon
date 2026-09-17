using UnityEngine;

public class SpaceShipCannon : MonoBehaviour
{
    public GameObject fireball;

    public float fireballSpeed;

    public int damage;

    public float duration;

    public float cooldown;

    public float currentCooldown;

    public bool isOnCooldown = false;

    public Sprite sprite;

    public Transform[] origin;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentCooldown = cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire()
    {
        if (!isOnCooldown) {
            InitiateFireball();
            isOnCooldown = true;
        }
    }

    private void InitiateFireball()
    {

        foreach (Transform t in origin)
        {
            Quaternion rotation = Quaternion.LookRotation(t.forward, t.up);

            var go = Instantiate(fireball, t.position, rotation);

            var fb = go.GetComponent<Fireball>();
            fb.origin = this.GetComponentInParent<SpaceEntity>().gameObject;
            fb.direction = rotation;
            fb.speed = fireballSpeed;
            fb.damage = damage;
            fb.duration = duration;
            

            go.GetComponent<SpriteRenderer>().sprite = sprite;
        }
        this.GetComponent<AudioSource>().Play();
    }

    private void FixedUpdate()
    {
        if (isOnCooldown)
        {
            currentCooldown += Time.fixedDeltaTime;
        }
        if(currentCooldown >= cooldown)
        {
            currentCooldown = 0;
            isOnCooldown = false;
        }
    }


}
