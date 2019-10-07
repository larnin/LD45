using UnityEngine;
using System.Collections;

public class Turet : MonoBehaviour
{
    const string joysticFireXAxis = "JoyFireX";
    const string joysticFireYAxis = "JoyFireY";
    const string mouseXAxis = "Mouse X";
    const string mouseYAxis = "Mouse Y";
    const string fireButton = "Fire1";
    const string fireAxis = "FireAxis";

    [SerializeField] GameObject m_projectile = null;
    [SerializeField] float m_turetRotationSpeed = 1;
    [SerializeField] float m_fireSpread = 1;
    [SerializeField] int m_fireCount = 1;
    [SerializeField] float m_fireDelta = 2;
    [SerializeField] float m_firPerSec = 2;
    [SerializeField] float m_projectileLife = 1;
    [SerializeField] float m_projectileSpeed = 1;
    
    [SerializeField] float m_fireOffset = 1;
    [SerializeField] int m_nbParticleOnFire = 1;

    Vector2 m_cursorPosition = new Vector2(0, 0);
    bool m_controlerWasCentredLastFrame = false;

    SubscriberList m_subscriberList = new SubscriberList();

    float m_oldFireAxis = 0;
    bool m_fire = false;
    float m_fireDelay = 0;
    float m_rotation = 0;

    Rigidbody m_rigidbody;

    Camera m_camera;

    ParticleSystem m_system;

    private void Awake()
    {
        m_rigidbody = GetComponentInParent<Rigidbody>();

        m_camera = Camera.main;

        m_system = GetComponentInChildren<ParticleSystem>();
        if(m_system != null)
            m_system.Pause();
    } 

    void Update()
    {
        var mousePos = Input.mousePosition;

        var ray = m_camera.ScreenPointToRay(Input.mousePosition);

        var plane = new Plane(new Vector3(0, 1, 0), Vector3.zero);

        float distance;
        plane.Raycast(ray, out distance);

        var targetPos = ray.origin + ray.direction * distance;

        var dir = new Vector2(targetPos.x - transform.position.x, targetPos.z - transform.position.z);

        float fire = 0;//Input.GetAxisRaw(fireAxis);
        if (((fire > 0.5f && m_oldFireAxis <= 0.5f) || Input.GetButtonDown(fireButton)) && Time.timeScale > 0.5f)
            StartFire();
        ProcessFire(dir);
        if ((fire < 0.5f && m_oldFireAxis >= 0.5f) || Input.GetButtonUp(fireButton))
            EndFire();
        m_oldFireAxis = fire;
    }
    
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
        
        float angle = Mathf.Atan2(dir.y, dir.x);
        float deltaAngle = MyMath.SignedDeltaAngleRad(m_rotation, angle);

        float dAngle = m_turetRotationSpeed * Mathf.Sign(deltaAngle) * Time.deltaTime;
        if (Mathf.Abs(dAngle) > Mathf.Abs(deltaAngle))
            dAngle = deltaAngle;

        m_rotation += dAngle;

        transform.rotation = Quaternion.Euler(0, - Mathf.Rad2Deg * m_rotation - 90, 0);
    }

    void EndFire()
    {
        m_fire = false;
    }

    void Fire()
    {
        float start = -m_fireDelta * (m_fireCount - 1) / 2;

        for (int i = 0; i < m_fireCount; i++)
        {
            float angle = start + m_fireDelta * i + m_rotation;

            var obj = Instantiate(m_projectile);

            var rigidbody = obj.GetComponent<Rigidbody>();

            var dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            if(rigidbody != null)
                rigidbody.velocity = new Vector3(dir.x, 0, dir.y) * m_projectileSpeed;

            var projectileTransform = obj.transform;
            
            var startPos = transform.position + new Vector3(Mathf.Cos(m_rotation), 0, Mathf.Sin(m_rotation)) * m_fireOffset;
            projectileTransform.position = startPos;

            projectileTransform.rotation = Quaternion.Euler(0, - Mathf.Rad2Deg * angle, 0);

            Destroy(obj, m_projectileLife);
        }

        if(m_system != null)
            m_system.Emit(m_nbParticleOnFire);
    }
}
