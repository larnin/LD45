using UnityEngine;
using System.Collections;

public class CarControler : MonoBehaviour
{
    static CarControler m_instance;

    public static CarControler instance
    {
        get { return m_instance; }
        private set
        {
            if (m_instance != null)
                Debug.LogWarning("2 CarControler instanciated");
            m_instance = value;
        }
    }
    string forwardInputName = "Vertical";
    string rotationInputName = "Horizontal";

    [SerializeField] float m_deadZone = 0.1f;
    [SerializeField] float m_maxForwardSpeed = 5;
    [SerializeField] float m_maxBackwardSpeed = 3;
    [SerializeField] float m_acceleration = 1;
    [SerializeField] float m_passiveDeceleration = 1;
    [SerializeField] float m_frontWheelRotation = Mathf.PI / 4;
    [SerializeField] float m_wheelDistance = 5;

    [SerializeField] float m_maxRotSpeedBeforeDrift = 1; // rotation angle in degre * speed
    [SerializeField] float m_driftDeceleration = 1;
    [SerializeField] float m_driftLostMaxSpeed = 1;

    float m_forwardInput = 0;
    float m_rotationInput = 0;

    float m_carTargetDirection = 0;
    float m_carDirection = 0;
    float m_speed = 0;
    bool m_moveForward = true;

    SubscriberList m_subscriberList = new SubscriberList();

    Rigidbody m_rigidbody = null;

    bool m_drifting = false;

    private void Awake()
    {
        instance = this;

        m_subscriberList.Add(new Event<BroadcastUpgradeEvent>.Subscriber(OnUpgrade));
        m_subscriberList.Subscribe();
    }

    private void OnDestroy()
    {
        m_subscriberList.Unsubscribe();
    }

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();

        Event<CameraInstantMoveEvent>.Broadcast(new CameraInstantMoveEvent(new Vector2(transform.position.x, transform.position.z), 0));
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

        float driftPower = Mathf.Deg2Rad * Mathf.DeltaAngle(Mathf.Rad2Deg * m_carDirection, Mathf.Rad2Deg * m_carTargetDirection);

        m_speed = m_rigidbody.velocity.magnitude * Mathf.Sign(m_speed);

        float maxSpeed = (m_speed > 0 ? m_maxForwardSpeed : m_maxBackwardSpeed) * 1 / (1 + m_driftLostMaxSpeed * driftPower);

        if (accelerate)
        {
            float speed = 0;
            if((m_forwardInput > 0) == (m_speed >0))
                speed = m_speed + m_acceleration / (Mathf.Abs(m_speed) + 1) * m_forwardInput * Time.deltaTime;
            else speed = m_speed + m_acceleration * m_forwardInput * Time.deltaTime;

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
        if (Mathf.Abs(m_speed) > maxSpeed)
        {
            float speed = m_speed - Mathf.Sign(m_speed) * m_driftDeceleration * Time.deltaTime * driftPower;
            if ((speed < 0) != (m_speed < 0))
                speed = 0;
            m_speed = speed;
        }

        backPos += m_speed * new Vector2(Mathf.Cos(m_carDirection), Mathf.Sin(m_carDirection)) * Time.deltaTime;
        frontPos += m_speed * new Vector2(Mathf.Cos(frontDirection), Mathf.Sin(frontDirection)) * Time.deltaTime;

        var dir = frontPos - backPos;

        float newDir = Mathf.Atan2(dir.y, dir.x);
        float deltaAngle = Mathf.Deg2Rad * MyMath.SignedDeltaAngle(Mathf.Rad2Deg * m_carDirection, Mathf.Rad2Deg * newDir);

        float maxRotAngle = m_maxRotSpeedBeforeDrift / Mathf.Max(Mathf.Abs(m_speed), 0.01f);

        m_carTargetDirection += deltaAngle;

        deltaAngle = Mathf.Deg2Rad * MyMath.SignedDeltaAngle(Mathf.Rad2Deg * m_carDirection, Mathf.Rad2Deg * m_carTargetDirection);

        if (deltaAngle < 0)
            m_carDirection += Mathf.Max(deltaAngle, -maxRotAngle);
        else m_carDirection += Mathf.Min(deltaAngle, maxRotAngle);

        if(Mathf.Abs(m_carTargetDirection - m_carDirection) > Mathf.PI * 1.5f)
        {
            m_carTargetDirection += Mathf.PI * 2 * Mathf.Sign(m_carDirection - m_carTargetDirection);
        }

        bool drifting = Mathf.Abs(m_carTargetDirection - m_carDirection) > 0.1f;
        if (m_drifting != drifting)
            Event<EnableDriftingEvent>.Broadcast(new EnableDriftingEvent(drifting));
        m_drifting = drifting;

        Vector3 velocity = new Vector3(Mathf.Cos(m_carDirection), 0, Mathf.Sin(m_carDirection)) * m_speed;

        transform.position = transform.position + velocity * Time.deltaTime;
        m_rigidbody.velocity = velocity;
        m_rigidbody.rotation = Quaternion.Euler(0, -Mathf.Rad2Deg * m_carTargetDirection, 0);
    }

    void OnUpgrade(BroadcastUpgradeEvent e)
    {
        m_maxForwardSpeed = e.upgrade.m_forwardSpeed;
        m_maxBackwardSpeed = e.upgrade.m_backwardSpeep;
        m_acceleration = e.upgrade.m_acceleration;
        m_passiveDeceleration = e.upgrade.m_passiveDeceleration;
        m_frontWheelRotation = e.upgrade.m_frontWheelRotation;
        m_wheelDistance = e.upgrade.m_wheelDistance;

        m_maxRotSpeedBeforeDrift = e.upgrade.m_maxRotSpeedBeforeDrift;
        m_driftDeceleration = e.upgrade.m_driftDeceleration;
        m_driftLostMaxSpeed = e.upgrade.m_driftLostMaxSpeed;
    }
}
