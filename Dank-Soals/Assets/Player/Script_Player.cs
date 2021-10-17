using UnityEngine;
using Cinemachine;
public class Script_Player : Script_CharacterMotor
{
    #region Singleton
    //
    private static Script_Player _instance;
    public static Script_Player Instance;
    //
    #endregion

    [SerializeField] float m_Health = 100.0f;
    [SerializeField] float m_SuccessiveDamageDelay;

    [Header("Internal Components")]
    [SerializeField] CinemachineVirtualCamera m_VCam;

    public GameObject[] m_WeaponWheel;

    #region Private Member Variables
    //
    float m_DamageTimer;
    //
    #endregion

    void Start()
    {
        InitCursorLockMode(CursorLockMode.Locked);
        InitMotor();
    }

    void Update()
    {
        Motor();
        UpdateDamageTimer();
        HotBarInput();
        Aim();
        Fire();
    }

    void LateUpdate()
    {
        m_SuccessiveDamageDelay = m_DodgeCooldown + 0.05f;
    }

    #region HP & Damage Control
    //
    public void TakeDamage(float _amount)
    {
        if (CanTakeDamage())
        {
            Debug.Log("Player Took : " + _amount + " : Damage");
            m_Health -= _amount;
            m_DamageTimer = m_SuccessiveDamageDelay;
        }
    }

    bool CanTakeDamage()
    {
        if (m_DamageTimer <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void UpdateDamageTimer()
    {
        if (m_DamageTimer <= 0)
        {
            m_DamageTimer = -1;
        }
        else
        {
            m_DamageTimer -= Time.deltaTime;
        }
    }
    //
    #endregion

    #region Weapon Wheel

    void HotBarInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (m_Animator)
            {
                if (m_Animator.GetBool("BeamGun"))
                {
                    m_Animator.SetBool("Pistol", false);
                    m_Animator.SetBool("BeamGun", false);
                    m_WeaponWheel[0].gameObject.SetActive(false);
                    m_WeaponWheel[1].gameObject.SetActive(false);
                }
                else
                {
                    m_Animator.SetBool("Pistol", false);
                    m_Animator.SetBool("BeamGun", true);
                    m_WeaponWheel[1].gameObject.SetActive(false);
                    m_WeaponWheel[0].gameObject.SetActive(true);
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (m_Animator)
            {
                if (m_Animator.GetBool("Pistol"))
                {
                    m_Animator.SetBool("BeamGun", false);
                    m_Animator.SetBool("Pistol", false);
                    m_WeaponWheel[0].gameObject.SetActive(false);
                    m_WeaponWheel[1].gameObject.SetActive(false);
                }
                else
                {
                    m_Animator.SetBool("BeamGun", false);
                    m_Animator.SetBool("Pistol", true);
                    m_WeaponWheel[0].gameObject.SetActive(false);
                    m_WeaponWheel[1].gameObject.SetActive(true);
                }
            }
        }
    }

    #endregion

    #region Combat

    void Aim()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            m_VCam.Priority = 11;
            if (m_Animator)
            {
                if (m_Animator.GetBool("Pistol") || m_Animator.GetBool("BeamGun"))
                {
                    if (!m_Animator.GetBool("Aiming"))
                    {
                        m_Animator.SetBool("Aiming", true);
                    }
                }
            }
        }
        else
        {
            m_VCam.Priority = 9;
            if (m_Animator)
            {
                m_Animator.SetBool("Aiming", false);
            }
        }
    }

    void Fire()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (m_Animator)
            {
                if (m_Animator.GetBool("Pistol") || m_Animator.GetBool("BeamGun"))
                {
                    if (m_Animator.GetBool("Aiming") && !m_Animator.GetBool("Fire"))
                    {
                        m_Animator.SetBool("Fire", true);
                    }
                }
            }
        }
        else
        {
            if (m_Animator)
            {
                m_Animator.SetBool("Fire", false);
            }
        }
    }

    #endregion
}
