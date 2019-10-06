using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using NRand;

public class SmallEnemyBehaviour : MonoBehaviour
{
    [SerializeField] float m_minIdleTime = 0;
    [SerializeField] float m_maxIdleTime = 0;
    [SerializeField] float m_moveDistanceMin = 0;
    [SerializeField] float m_moveDistanceMax = 0;

    NavMeshAgent m_agent;

    bool m_moving = false;
    float m_duration = 0;

    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        m_moving = false;
        m_duration = 0;
        if (m_agent != null)
            m_agent.destination = transform.position;
    }

    void Update()
    {
        if(!m_moving)
        {
            if (m_duration <= 0)
            {
                float dist = new UniformFloatDistribution(m_moveDistanceMin, m_moveDistanceMax).Next(new StaticRandomGenerator<MT19937>());
                float angle = new UniformFloatDistribution(0, Mathf.PI * 2).Next(new StaticRandomGenerator<MT19937>());

                var pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * dist;
                pos += transform.position;

                m_agent.destination = pos;

                m_moving = true; 
            }

            m_duration -= Time.deltaTime;
        }
        else
        {
            if(m_agent.remainingDistance < 1)
            {
                m_moving = false;
                m_duration = new UniformFloatDistribution(m_minIdleTime, m_maxIdleTime).Next(new StaticRandomGenerator<MT19937>());
            }
        }
    }
}
