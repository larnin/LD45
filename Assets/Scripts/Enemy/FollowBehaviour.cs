using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class FollowBehaviour : MonoBehaviour
{
    [SerializeField] float m_detectionRarius = 0;
    [SerializeField] float m_lostRadius = 0;
    [SerializeField] List<Behaviour> m_disableBehavioursWhenActive = new List<Behaviour>();

    bool m_follow = false;
    float m_updateTargetTimer = 0;

    NavMeshAgent m_agent;

    private void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        m_follow = false;
        if(m_agent != null)
            m_agent.destination = transform.position;
    }

    private void OnDisable()
    {
        foreach (var c in m_disableBehavioursWhenActive)
            c.enabled = false;
    }

    void Update()
    {
        if(CarControler.instance == null)
        {
            foreach (var c in m_disableBehavioursWhenActive)
                c.enabled = true;
            return;
        }

        float radius = m_follow ? m_detectionRarius : m_lostRadius;

        var pos = new Vector2(transform.position.x, transform.position.z);
        var target = new Vector2(CarControler.instance.transform.position.x, CarControler.instance.transform.position.z);

        var dist = (target - pos).magnitude;

        if(dist > radius)
        {
            foreach (var c in m_disableBehavioursWhenActive)
                c.enabled = true;
            m_updateTargetTimer = 0;
            return;
        }

        foreach (var c in m_disableBehavioursWhenActive)
            c.enabled = false;

        if(m_updateTargetTimer <= 0)
        {
            m_updateTargetTimer = 0.25f;

            m_agent.destination = new Vector3(target.x, 0, target.y);
        }
        m_updateTargetTimer -= Time.deltaTime;
    }
}
