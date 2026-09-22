using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SpaceShipCannon : MonoBehaviour
{
    public SpaceCannon cannon;

    public Transform[] origin;

    public float currentCooldown;
    public bool isOnCooldown = false;

    private AudioSource audio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audio = GetComponent<AudioSource>();
        SetCannon(cannon);
    }

    public void SetCannon(SpaceCannon c)
    {
        if(c == null)
        {
            this.cannon = null;
            return;
        }
        var audio = GetComponent<AudioSource>();
        currentCooldown = c.cooldown;
        audio.volume = c.volume;
        audio.pitch = c.pitch;
        audio.generator = c.audio;
        this.cannon = c;
    }

    public void Fire()
    {
        if (cannon == null)
        {
            return;
        }
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

            var go = Instantiate(cannon.fireball, t.position, rotation);

            var fb = go.GetComponent<Fireball>();
            fb.origin = GetComponentInParent<SpaceEntity>().gameObject;
            fb.direction = direction;
            fb.speed = cannon.fireballSpeed;
            fb.damage = cannon.damage;
            fb.duration = cannon.duration;

            go.GetComponent<SpriteRenderer>().sprite = cannon.sprite;
        }
        audio.Play();
    }

    private void FixedUpdate()
    {
        if (isOnCooldown)
        {
            currentCooldown += Time.fixedDeltaTime;
        }
        if(cannon != null && currentCooldown >= cannon.cooldown)
        {
            currentCooldown = 0;
            isOnCooldown = false;
        }
    }

    public bool IsInRange(Transform target)
    {
        if (cannon == null || target == null)
            return false;

        float range = cannon.fireballSpeed * cannon.duration;

        Vector2 originPosition = origin != null && origin.Length > 0
            ? origin[0].position
            : transform.position;

        Vector2 offset = (Vector2)target.position - originPosition;

        return offset.sqrMagnitude <= range * range;
    }


}
