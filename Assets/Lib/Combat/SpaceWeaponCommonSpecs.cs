using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "Combat/SpaceWeaponCommonSpecs")]
public class SpaceWeaponCommonSpecs : ScriptableObject
{
    [Header("Specs")]
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
}

