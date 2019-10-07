using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using NRand;

public class PathNodeBehaviour : MonoBehaviour
{
    [SerializeField] float m_minIdleTime = 0;
    [SerializeField] float m_maxIdleTime = 0;

    bool m_moving = false;
    float m_duration = 0;

    NavMeshAgent m_agent;

    private void Start()
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

    private void Update()
    {
        if (m_moving)
        {
            if (m_agent.remainingDistance < 1)
            {
                m_moving = false;
                m_duration = new UniformFloatDistribution(m_minIdleTime, m_maxIdleTime).Next(new StaticRandomGenerator<MT19937>());
            }
        }
        else
        {
            if (m_duration <= 0)
            {
                var target = PathNode.GetRandomNode();
                if (target != null)
                {
                    m_agent.destination = target.transform.position;

                    m_moving = true;
                }
            }

            m_duration -= Time.deltaTime;
        }
    }
}