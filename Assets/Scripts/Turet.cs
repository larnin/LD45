using UnityEngine;
using System.Collections;

public class Turet : MonoBehaviour
{
    const string joysticFireXAxis = "JoyFireX";
    const string joysticFireYAxis = "JoyFireY";
    const string mouseXAxis = "MouseX";
    const string mouseYAxis = "MouseY";
    const string fireButton = "Fire";
    const string fireAxis = "FireAxis";

    [SerializeField] GameObject m_projectile = null;
    [SerializeField] float m_turetRotationSpeed = 1;
    [SerializeField] float m_fireSpread = 1;
    [SerializeField] int m_fireCount = 1;
    [SerializeField] float m_fireDelta = 2;
    [SerializeField] float m_firPerSec = 2;

    [SerializeField] float m_threshold = 0.1f;
    [SerializeField] float m_cursorSpeed = 1;
    [SerializeField] float m_cursorMaxDistance = 1;

    Vector2 m_cursorPosition = new Vector2(0, 0);
    bool m_controlerWasCentredLastFrame = false;

    SubscriberList m_subscriberList = new SubscriberList();

    float m_oldFireAxis = 0;

    Rigidbody m_rigidbody;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        UpdateMouseCursor();
        UpdateControlerCursor();

        Vector2 pos = transform.position;
        Event<WeaponTargetChangeEvent>.Broadcast(new WeaponTargetChangeEvent(m_cursorPosition + pos, m_cursorPosition.magnitude));

        var velocity = new Vector2(m_rigidbody.velocity.x, m_rigidbody.velocity.z);

        var dir = m_cursorPosition;
        if (dir.sqrMagnitude < 0.01f)
            dir = velocity.normalized;


        float fire = Input.GetAxisRaw(fireAxis);
        if (((fire > 0.5f && m_oldFireAxis <= 0.5f) || Input.GetButtonDown(fireButton)) && Time.timeScale > 0.5f)
            StartFire();
        ProcessFire(dir);
        if ((fire < 0.5f && m_oldFireAxis >= 0.5f) || Input.GetButtonUp(fireButton))
            EndFire();
        m_oldFireAxis = fire;
    }

    void UpdateMouseCursor()
    {
        Vector2 offset = new Vector2(Input.GetAxisRaw(mouseXAxis), Input.GetAxisRaw(mouseYAxis));

        if (Time.timeScale < 0.5f)
            return;

        if (Mathf.Abs(offset.x) <= 0 || Mathf.Abs(offset.y) <= 0)
            return;

        offset *= m_cursorSpeed;

        m_cursorPosition += offset;

        float magnitude = m_cursorPosition.magnitude;
        if (magnitude > m_cursorMaxDistance)
            m_cursorPosition *= m_cursorMaxDistance / magnitude;
    }

    void UpdateControlerCursor()
    {
        Vector2 dir = new Vector2(Input.GetAxisRaw(joysticFireXAxis), -Input.GetAxisRaw(joysticFireYAxis));

        if (dir.sqrMagnitude < m_threshold * m_threshold)
        {
            if (m_controlerWasCentredLastFrame)
                return;
            m_controlerWasCentredLastFrame = true;
            dir = new Vector2(0, 0);
        }
        else m_controlerWasCentredLastFrame = false;

        m_cursorPosition = dir * m_cursorMaxDistance;
    }

    bool m_fire = false;
    float m_fireDelay = 0;
    float m_rotation = 0;

    void StartFire()
    {
        m_fireDelay = 0;
        m_fire = true;
    }

    void ProcessFire(Vector2 dir)
    {
        m_fireDelay -= Time.deltaTime;

        if(m_fireDelay <= 0 && m_fire)
        {
            Fire();
            m_fireDelay = 1 / m_firPerSec;
        }

        dir = transform.parent.InverseTransformDirection(dir);

        float angle = Mathf.Atan2(dir.y, dir.x);
        float deltaAngle = MyMath.SignedDeltaAngle(m_rotation, angle);

        float dAngle = m_turetRotationSpeed * Mathf.Sign(deltaAngle) * Time.deltaTime;
        if (Mathf.Abs(dAngle) > Mathf.Abs(deltaAngle))
            dAngle = deltaAngle;

        m_rotation += dAngle;

        transform.localRotation = Quaternion.Euler(0, Mathf.Rad2Deg * m_rotation, 0);
    }

    void EndFire()
    {
        m_fire = false;
    }

    void Fire()
    {

    }
}
