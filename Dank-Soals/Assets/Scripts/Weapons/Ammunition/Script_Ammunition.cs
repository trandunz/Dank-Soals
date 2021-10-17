using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Ammunition : MonoBehaviour
{
    public float m_TravelSpeed = 1.0f;

    [SerializeField] float m_Damage = 1.0f;
    [SerializeField] AudioClip m_ImpactSound;

    bool MARKASDESTROY = false;

    void OnCollisionEnter(Collision collision)
    {
        if (!MARKASDESTROY)
        {
            StartCoroutine(Impact());
        }
    }

    IEnumerator Impact()
    {
        MARKASDESTROY = true;
        GetComponent<MeshRenderer>().forceRenderingOff = true;
        GetComponentInChildren<ParticleSystem>().Stop();
        GetComponent<AudioSource>().PlayOneShot(m_ImpactSound);
        yield return new WaitForSeconds(m_ImpactSound.length);
        Destroy(gameObject);
    }
}
