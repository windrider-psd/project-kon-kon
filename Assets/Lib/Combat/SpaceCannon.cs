using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "Combat/SpaceWeaponCommonSpecs")]
public class SpaceCannon : ScriptableObject
{
    [Header("Specs")]
    public SpaceCannonClassId id;
    public string weaponName;
    public float fireballSpeed;

    public int damage;

    public float duration;

    public float cooldown;

    [Header("Audio")]
    public AudioClip audio;
    public float pitch;
    public float volume;

    [Header("Rendering")]
    public GameObject fireball;
    public Sprite sprite;

    [Header("Compatibility")]
    public GearSize size;
    public bool fitsOnTurret;
    public bool fitsOnCannon;
}

