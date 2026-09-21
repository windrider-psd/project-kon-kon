using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "Gear/Cannon")]
public class SpaceCannon : Gear
{
    [Header("Specs")]
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
    public bool fitsOnTurret;
    public bool fitsOnCannon;
}

