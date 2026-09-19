using UnityEngine;
using UnityEngine.U2D;
using static UnityEngine.UI.Image;

[RequireComponent(typeof(AudioSource))]
public class SpaceShipTurret : MonoBehaviour
{
    public SpaceWeaponCommonSpecs specs;
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
        currentCooldown = specs.cooldown;

        audio = GetComponent<AudioSource>();
        audio.volume = specs.volume;
        audio.pitch = specs.pitch;
        audio.generator = specs.audio;
    }

    private void Update()
    {
        Fire();
    }

    public void Fire()
    {
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
            specs.fireball,
            transform.position,
            Quaternion.identity
        );

        var fb = go.GetComponent<Fireball>();

        fb.origin = GetComponentInParent<SpaceEntity>().gameObject;
        fb.direction = direction;
        fb.speed = specs.fireballSpeed;
        fb.damage = specs.damage;
        fb.duration = specs.duration;
        fb.hardTarget = Target.gameObject;
        go.GetComponent<SpriteRenderer>().sprite = specs.sprite;

        audio.Play();

    }

    private void FixedUpdate()
    {
        if (isOnCooldown)
        {
            currentCooldown += Time.fixedDeltaTime;
        }
        if (currentCooldown >= specs.cooldown)
        {
            currentCooldown = 0;
            isOnCooldown = false;
        }
    }
}
