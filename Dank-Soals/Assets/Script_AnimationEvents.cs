using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_AnimationEvents : MonoBehaviour
{
    Script_Player m_Player;

    public void Fire(string _name)
    {
        m_Player = GetComponentInParent<Script_Player>();
        switch (_name)
        {
            case "BeamGun":
                {
                    ScriptableObject_Weapon m_WeaponData = m_Player.m_WeaponWheel[0].GetComponent<Script_Weapon>().m_WeaponData;
                    m_Player.m_WeaponWheel[0].GetComponent<AudioSource>().PlayOneShot(m_WeaponData.UseSound);

                    Rigidbody bullet = Instantiate(m_Player.m_WeaponWheel[0].GetComponent<Script_Weapon>().m_Bullet, m_Player.m_WeaponWheel[0].GetComponent<Script_Weapon>().m_Muzzle.transform.position, m_Player.m_WeaponWheel[0].GetComponent<Script_Weapon>().m_Muzzle.transform.rotation);
                    bullet.velocity = m_Player.m_WeaponWheel[0].GetComponent<Script_Weapon>().m_Muzzle.transform.TransformDirection(new Vector3(0.0f, 0.0f, m_WeaponData.Bullet.GetComponent<Script_Ammunition>().m_TravelSpeed));
                    break;
                }
            case "Pistol":
                {
                    ScriptableObject_Weapon m_WeaponData = m_Player.m_WeaponWheel[1].GetComponent<Script_Weapon>().m_WeaponData;
                    m_Player.m_WeaponWheel[1].GetComponent<AudioSource>().PlayOneShot(m_WeaponData.UseSound);

                    Rigidbody bullet = Instantiate(m_Player.m_WeaponWheel[1].GetComponent<Script_Weapon>().m_Bullet, m_Player.m_WeaponWheel[1].GetComponent<Script_Weapon>().m_Muzzle.transform.position, m_Player.m_WeaponWheel[1].GetComponent<Script_Weapon>().m_Muzzle.transform.rotation);
                    bullet.velocity = m_Player.m_WeaponWheel[1].GetComponent<Script_Weapon>().m_Muzzle.transform.TransformDirection(new Vector3(0.0f, 0.0f, m_WeaponData.Bullet.GetComponent<Script_Ammunition>().m_TravelSpeed));
                    break;
                }
            default:
                break;
        }
    }
}
