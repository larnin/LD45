using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class RotateToPlayerBehaviour : MonoBehaviour
{
    [SerializeField] float m_detectionRarius = 0;
    [SerializeField] float m_rotationSpeed = 0;
    [SerializeField] List<Behaviour> m_disableBehavioursWhenActive = new List<Behaviour>();

    NavMeshAgent m_agent;

    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
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
        if (CarControler.instance == null)
        {
            foreach (var c in m_disableBehavioursWhenActive)
                c.enabled = true;
            return;
        }

        var pos = new Vector2(transform.position.x, transform.position.z);
        var target = new Vector2(CarControler.instance.transform.position.x, CarControler.instance.transform.position.z);

        var dist = (target - pos).magnitude;

        if(dist < m_detectionRarius)
        {
            m_agent.updateRotation = false;
            foreach (var c in m_disableBehavioursWhenActive)
                c.enabled = false;

            float angle = -transform.rotation.eulerAngles.y + 90;
            var dir = target - pos;
            float targetAngle = Mathf.Rad2Deg * Mathf.Atan2(dir.y, dir.x);

            float deltaAngle = MyMath.SignedDeltaAngle(angle, targetAngle);
            float dtAngle = Time.deltaTime * m_rotationSpeed * Mathf.Sign(deltaAngle);
            if (Mathf.Abs(dtAngle) > Mathf.Abs(deltaAngle))
                dtAngle = deltaAngle;
            
            angle += dtAngle;

            transform.rotation = Quaternion.Euler(0, -angle + 90, 0);
        }
        else
        {
            m_agent.updateRotation = true;
            foreach (var c in m_disableBehavioursWhenActive)
                c.enabled = true;
        }
    }
}
