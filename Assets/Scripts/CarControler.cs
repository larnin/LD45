using UnityEngine;
using System.Collections;

public class CarControler : MonoBehaviour
{
    string forwardInputName = "Vertical";
    string rotationInputName = "Horizontal";

    [SerializeField] float m_deadZone = 0.1f;
    [SerializeField] float m_maxForwardSpeed = 5;
    [SerializeField] float m_maxBackwardSpeed = 3;
    [SerializeField] float m_acceleration = 5;
    [SerializeField] float m_passiveDeceleration = 1;
    [SerializeField] float m_maneuverability = 10;
    [SerializeField] float m_frontWheelRotation = Mathf.PI / 4;
    [SerializeField] float m_wheelDistance = 5;

    float m_forwardInput = 0;
    float m_rotationInput = 0;

    float m_carDirection = 0;
    float m_speed = 0;
    bool m_moveForward = true;

    Rigidbody2D m_rigidbody = null;

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        var dir = new Vector2(Input.GetAxisRaw(rotationInputName), Input.GetAxisRaw(forwardInputName));
        float dirLenght = dir.magnitude;
        if (dirLenght != 0)
        {
            dir /= dirLenght;
            if (dirLenght < m_deadZone)
                dirLenght = 0;
            else dirLenght = (dirLenght - m_deadZone) / (1 - m_deadZone);
            dir *= dirLenght;
        }

        if(m_forwardInput == 0 && dir.y != 0)
        {
            if (m_speed == 0)
                m_moveForward = dir.y > 0;
        }

        m_rotationInput = dir.x;
        m_forwardInput = dir.y;
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;

        Vector2 backPos = pos - m_wheelDistance * new Vector2(Mathf.Cos(m_carDirection), Mathf.Sin(m_carDirection));
        Vector2 frontPos = pos + m_wheelDistance * new Vector2(Mathf.Cos(m_carDirection), Mathf.Sin(m_carDirection));

        float frontDirection = m_carDirection + m_rotationInput * m_frontWheelRotation;

        bool accelerate = m_forwardInput != 0;

        if (accelerate)
        {
            float speed = m_speed + m_acceleration * m_forwardInput * Time.deltaTime;
            if (speed > m_maxForwardSpeed)
                speed = m_maxForwardSpeed;
            if (speed < -m_maxBackwardSpeed)
                speed = -m_maxBackwardSpeed;
            if (m_moveForward && speed < 0)
                speed = 0;
            if (!m_moveForward && speed > 0)
                speed = 0;
            m_speed = speed;
        }
        else
        {
            float speed = m_speed - Mathf.Sign(m_speed) * m_passiveDeceleration * Time.deltaTime;
            if ((speed < 0) != (m_speed < 0))
                speed = 0;
            m_speed = speed;
        }

        backPos += m_speed * new Vector2(Mathf.Cos(m_carDirection), Mathf.Sin(m_carDirection)) * Time.deltaTime;
        frontPos += m_speed * new Vector2(Mathf.Cos(frontDirection), Mathf.Sin(frontDirection)) * Time.deltaTime;

        var dir = frontPos - backPos;

        Vector2 velocity = dir.normalized * m_speed;

        m_carDirection = Mathf.Atan2(dir.y, dir.x);

        m_rigidbody.velocity = velocity;
        m_rigidbody.rotation = Mathf.Rad2Deg * m_carDirection;
    }
}
