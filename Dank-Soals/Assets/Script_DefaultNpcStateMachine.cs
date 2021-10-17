using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Script_DefaultNpcStateMachine : MonoBehaviour
{
    public enum StateMachine
    {
        WANDER,
        CHASE,
        INCOMBAT
    }

    [Header("Current State")]
    [SerializeField] private StateMachine m_CurrentStatemachine;

    [Header("Internal Components")]
    [SerializeField] private NavMeshAgent m_NavMeshAgent;
    [SerializeField] private LayerMask m_GroundMask, m_PlayerMask;
    private Transform m_PlayerTransform;
    
    [Header("Wander Behaviour")]
    [SerializeField] private Vector3 m_WanderPoint;
    [SerializeField] private float m_WanderExtents;
    private bool m_IsWalkPointSet = false;

    [Header("Sensors & Timers")]
    [SerializeField] private float m_SightRange;
    [SerializeField] private Vector2 m_ChaseTimer = new Vector2(0.0f, 5.0f); // <----Vector2(CurrentTime, MaxTime);
    RaycastHit hit;
    bool m_IsPlayerInSight = false;
    bool m_isPlayerInAttackRange  = false;

    void Wander()
    {
        if (!m_IsWalkPointSet)
        {
            GenerateWalkPos();
        }
        else
        {
            m_NavMeshAgent.SetDestination(m_WanderPoint);
        }
        Vector3 distanceToWalkPoint = transform.position - m_WanderPoint;
        if (distanceToWalkPoint.sqrMagnitude < 1)
        {
            m_IsWalkPointSet = false;
        }
    }
    void Pursuit()
    {
        if (m_ChaseTimer.x > 0)
        { 
            // Something like this
            // m_NavMeshAgent.SetDestination(Player.position)
        }
    }

    void LineOfSight()
    {
        if (m_ChaseTimer.x > 0.0f)
        {
            m_ChaseTimer.x -= Time.deltaTime;
        }
       
       
        if (Physics.Raycast(transform.position + Vector3.up, m_PlayerTransform.position - transform.position, out hit, m_SightRange))
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.CompareTag("Player"))
            {
                m_IsPlayerInSight = true;
                m_ChaseTimer.x = m_ChaseTimer.y;
              
            }
            else
            {
                m_IsPlayerInSight = false;
            }
        }

        if (m_ChaseTimer.x < 0)
        {
            m_IsPlayerInSight = false;
        }
    }

    void GenerateWalkPos()
    {
        float randomZ = Random.Range(-m_WanderExtents, m_WanderExtents);
        float randomX = Random.Range(-m_WanderExtents, m_WanderExtents);

        m_WanderPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(m_WanderPoint, Vector3.down, 2f, m_GroundMask))
        {
            m_IsWalkPointSet = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        m_CurrentStatemachine = StateMachine.WANDER;
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
       /* m_isPlayerInAttackRange = Physics.CheckSphere(transform.position, 10.0f, m_PlayerMask);
        m_IsPlayerInSight = Physics.CheckSphere(transform.position, m_SightRange, m_PlayerMask);*/

       /* LineOfSight();*/

        if (!m_IsPlayerInSight && !m_isPlayerInAttackRange) Wander();
    }

    private void OnDrawGizmosSelected()
    {
        if (m_WanderPoint != Vector3.zero)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, m_WanderPoint);
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_SightRange);
    }
}
