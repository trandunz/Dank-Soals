using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/Weapon")]
public class ScriptableObject_Weapon : ScriptableObject
{
    public string Name;
    public GameObject ParticleSystem;
    public GameObject Bullet;
    public AudioClip UseSound;
}
