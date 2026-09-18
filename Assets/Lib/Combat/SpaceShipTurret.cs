using UnityEngine;
using UnityEngine.U2D;
using static UnityEngine.UI.Image;

public class SpaceShipTurret : MonoBehaviour
{
    public SpaceWeaponFireballLooks looks;
    public SpaceWeaponSpecs specs;
    public float currentCooldown;
    public bool isOnCooldown = false;

    private AISpaceShipController controller;
    

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
            looks.fireball,
            transform.position,
            Quaternion.identity
        );

        var fb = go.GetComponent<Fireball>();

        fb.origin = GetComponentInParent<SpaceEntity>().gameObject;
        fb.direction = direction;
        fb.speed = specs.fireballSpeed;
        fb.damage = specs.damage;
        fb.duration = specs.duration;

        go.GetComponent<SpriteRenderer>().sprite = looks.sprite;

        GetComponent<AudioSource>().Play();
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
