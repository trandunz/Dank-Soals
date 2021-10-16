using UnityEngine;

public class Script_CharacterMotor : MonoBehaviour
{
    [SerializeField] LayerMask m_GroundLayer;
    [SerializeField] CharacterController m_CharacterController;
    [SerializeField] Transform m_MainCamera;
    [SerializeField] Animator m_Animator;

    [SerializeField] float m_Mass = 60.0f;
    [SerializeField] float m_MovementSpeed = 6.0f;
    [SerializeField] float m_SmoothTurnTime;

    #region Dodge
    //
    [SerializeField] protected float m_DodgeCooldown = 1.0f;
    [SerializeField] float m_DodgeLength = 3.0f;
    float m_ActCooldown;
    //
    #endregion

    #region Private Member Variables
    //
    float m_SmoothTurnVelocity;
    Vector3 m_Impact = Vector3.zero;
    float m_DistanceToGround;
    //
    #endregion

    protected void Motor()
    {
        Movement();
        CursorResetCheck();
        Jump();
        HandleImpacts();
    }

    protected void InitMotor()
    {
        InitDistanceToGround();
    }

    protected void InitCursorLockMode(CursorLockMode _cursorMode)
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    #region Private Functions
    //
    void InitDistanceToGround()
    {
        m_DistanceToGround = m_CharacterController.bounds.extents.y;
    }

    void Movement()
    {
        if (!IsUsingUI())
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(horizontal, 0.0f, vertical).normalized;

            m_Animator.SetFloat("Speed", direction.magnitude);

            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + m_MainCamera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref m_SmoothTurnVelocity, m_SmoothTurnTime);
                transform.rotation = Quaternion.Euler(0, angle, 0.0f);

                Vector3 moveDirection = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;
                m_CharacterController.Move(moveDirection.normalized * m_MovementSpeed * Time.deltaTime);
            }
        }
    }

    void Jump()
    {
        if (m_ActCooldown <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Player Jumped!");
                //m_Animator.SetBool("IsJumping", true);

                AddImpact(transform.forward, m_DodgeLength * 50);

                m_ActCooldown = m_DodgeCooldown;
            }
        }

        m_ActCooldown -= Time.deltaTime;
    }

    void AddImpact(Vector3 _force, float _strenth)
    {
        _force.Normalize();
        if (_force.y < 0)
        {
            _force.y = -_force.y;
        }

        m_Impact += _force.normalized * _strenth / m_Mass;
    }

    void HandleImpacts()
    {
        if (m_Impact.magnitude > 0.2f)
        {
            m_CharacterController.Move(m_Impact * m_MovementSpeed * Time.deltaTime);
        }

        m_Impact = Vector3.Lerp(m_Impact, Vector3.zero, 5 * Time.deltaTime);
    }

    void OnGUI()
    {
        //Press this button to lock the Cursor
        if (GUI.Button(new Rect(0, 0, 100, 50), "Lock Cursor"))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        //Press this button to confine the Cursor within the screen
        if (GUI.Button(new Rect(125, 0, 100, 50), "Confine Cursor"))
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    void CursorResetCheck()
    {
        //Press the space bar to apply no locking to the Cursor
        if (Input.GetKey(KeyCode.Tab))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    bool IsUsingUI()
    {
        if (Cursor.lockState == CursorLockMode.None)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    //
    #endregion
}
