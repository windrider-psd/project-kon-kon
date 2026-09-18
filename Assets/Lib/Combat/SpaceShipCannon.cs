using UnityEngine;

public class SpaceShipCannon : MonoBehaviour
{
    public SpaceWeaponFireballLooks looks;
    public SpaceWeaponSpecs specs;

    public Transform[] origin;

    public float currentCooldown;
    public bool isOnCooldown = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentCooldown = specs.cooldown;
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
            Vector3 direction = t.up;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            var go = Instantiate(looks.fireball, t.position, rotation);

            var fb = go.GetComponent<Fireball>();
            fb.origin = GetComponentInParent<SpaceEntity>().gameObject;
            fb.direction = direction;
            fb.speed = specs.fireballSpeed;
            fb.damage = specs.damage;
            fb.duration = specs.duration;

            go.GetComponent<SpriteRenderer>().sprite = looks.sprite;
        }
        this.GetComponent<AudioSource>().Play();
    }

    private void FixedUpdate()
    {
        if (isOnCooldown)
        {
            currentCooldown += Time.fixedDeltaTime;
        }
        if(currentCooldown >= specs.cooldown)
        {
            currentCooldown = 0;
            isOnCooldown = false;
        }
    }


}
