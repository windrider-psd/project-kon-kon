using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SpaceShipCannon : MonoBehaviour
{
    public SpaceWeaponCommonSpecs specs;

    public Transform[] origin;

    public float currentCooldown;
    public bool isOnCooldown = false;

    private AudioSource audio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentCooldown = specs.cooldown;
        audio = GetComponent<AudioSource>();
        audio.volume = specs.volume;
        audio.pitch = specs.pitch;
        audio.generator = specs.audio;
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

            var go = Instantiate(specs.fireball, t.position, rotation);

            var fb = go.GetComponent<Fireball>();
            fb.origin = GetComponentInParent<SpaceEntity>().gameObject;
            fb.direction = direction;
            fb.speed = specs.fireballSpeed;
            fb.damage = specs.damage;
            fb.duration = specs.duration;

            go.GetComponent<SpriteRenderer>().sprite = specs.sprite;
        }
        audio.Play();
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
