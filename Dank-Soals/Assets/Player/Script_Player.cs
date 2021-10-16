using UnityEngine;

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
}
