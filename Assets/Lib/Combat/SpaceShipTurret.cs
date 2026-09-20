using UnityEngine;
using UnityEngine.U2D;
using static UnityEngine.UI.Image;

[RequireComponent(typeof(AudioSource))]
public class SpaceShipTurret : MonoBehaviour
{
    public SpaceCannon cannon;
    public float currentCooldown;
    public bool isOnCooldown = false;

    private AISpaceShipController controller;

    private AudioSource audio;
    private Transform Target
    {
        get
        {
            if(controller == null || controller.currentOrder != AISpaceShipOrder.Kill || controller.target == null)
            {
                return null;
            }
            return controller.target.transform;
        }
    }

    void Start()
    {
        controller = GetComponentInParent<AISpaceShipController>();
        

        audio = GetComponent<AudioSource>();

        SetCannon(cannon);
    }

    public void SetCannon(SpaceCannon c)
    {
        if (c == null)
        {
            this.cannon = null;
            return;
        }
        audio = GetComponent<AudioSource>();
        currentCooldown = c.cooldown;
        audio.volume = c.volume;
        audio.pitch = c.pitch;
        audio.generator = c.audio;
        this.cannon = c;
    }

    private void Update()
    {
        Fire();
    }

    public void Fire()
    {
        if (cannon == null)
        {
            return;
        }
        if (!isOnCooldown && Target != null)
        {

            InitiateFireball();
            isOnCooldown = true;
        }
    }

    private void InitiateFireball()
    {
        Vector3 direction = (Target.position - transform.position).normalized;

        var go = Instantiate(
            cannon.fireball,
            transform.position,
            Quaternion.identity
        );

        var fb = go.GetComponent<Fireball>();

        fb.origin = GetComponentInParent<SpaceEntity>().gameObject;
        fb.direction = direction;
        fb.speed = cannon.fireballSpeed;
        fb.damage = cannon.damage;
        fb.duration = cannon.duration;
        fb.hardTarget = Target.gameObject;
        go.GetComponent<SpriteRenderer>().sprite = cannon.sprite;

        audio.Play();

    }

    private void FixedUpdate()
    {
        if (isOnCooldown)
        {
            currentCooldown += Time.fixedDeltaTime;
        }
        if (cannon != null && currentCooldown >= cannon.cooldown)
        {
            currentCooldown = 0;
            isOnCooldown = false;
        }
    }
}
