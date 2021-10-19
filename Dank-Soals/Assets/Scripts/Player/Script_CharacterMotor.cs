using UnityEngine;

public class Script_CharacterMotor : MonoBehaviour
{
    [SerializeField] LayerMask m_GroundLayer;
    [SerializeField] CharacterController m_CharacterController;
    [SerializeField] Transform m_MainCamera;
    [SerializeField] protected Animator m_Animator;

    [SerializeField] float m_Mass;
    [SerializeField] float m_MovementSpeed;
    [SerializeField] float m_JumpPower;
    [SerializeField] float m_SmoothTurnTime;
    [SerializeField] float  m_ForceScale;

    #region Jump

    [SerializeField] float m_MaxJumpTime;

    #endregion

    #region Dodge
    //
    [SerializeField] protected float m_DodgeCooldown = 1.0f;
    [SerializeField] float m_DodgeLength = 3.0f;
    float m_ActCooldown = 1.0f;
    //
    #endregion

    #region Private Member Variables
    //
    float m_SmoothTurnVelocity;
    Vector3 m_Impact = Vector3.zero;
    float m_DistanceToGround;
    bool m_IsJumping = false;
    float m_JumpTime = 0;
    //
    #endregion

    public Animator GetAnimator()
    {
        return m_Animator;
    }

    protected void Motor()
    {
        Movement();
        CursorResetCheck();
        Roll();
        Jump();
        Gravity();
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

    void Roll()
    {
        if (m_ActCooldown <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Player Rolled!");
                m_Animator.SetBool("Dodge", true);
                AddImpact(transform.forward, m_DodgeLength);

                m_ActCooldown = m_DodgeCooldown;
            }
        }
        else
        {

            m_ActCooldown -= Time.deltaTime;
        }
    }

    void Jump()
    {
        if (m_ActCooldown <= 0)
        {
            if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
            {
                m_IsJumping = true;
                AddImpact(Vector3.up, m_JumpPower * 1.5f);
                AddImpact(m_Animator.gameObject.transform.forward, m_DodgeLength / 2);
            }

            if (Input.GetKey(KeyCode.Space) && m_IsJumping)
            {
                if (m_JumpTime < m_MaxJumpTime)
                {
                    AddImpact(Vector3.up, m_JumpPower / 6);
                    AddImpact(m_Animator.gameObject.transform.forward, m_DodgeLength / 24);
                    m_JumpTime += Time.deltaTime;
                }
                else
                {
                    m_JumpTime = 0;
                    m_IsJumping = false;
                }
            }
            else
            {
                m_IsJumping = false;
                m_JumpTime = 0;
            }
        }
    }

    void Gravity()
    {
        AddImpact(Vector3.down, - 1);
    }

    void AddImpact(Vector3 _force, float _strenth)
    {
        _force.Normalize();
        if (_force.y < -0.01f)
        {
            _force.y = -_force.y;
        }

        m_Impact += _force.normalized * _strenth;
    }

    void HandleImpacts()
    {
        if (m_Impact.magnitude > 0.1f)
        {
            m_CharacterController.Move(m_Impact * m_Mass * m_ForceScale * Time.deltaTime);
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

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, m_DistanceToGround + 0.1f);
    }
    //
    #endregion
}
