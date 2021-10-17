using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Ammunition : MonoBehaviour
{
    public float m_TravelSpeed = 1.0f;
    [SerializeField] float m_Damage = 1.0f;

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
