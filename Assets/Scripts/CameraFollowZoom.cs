using UnityEngine;
using System.Collections;

public class CameraFollowZoom : MonoBehaviour
{
    [SerializeField] AnimationCurve m_fovOffsetOnMoveSpeed = null;
    [SerializeField] float m_fovMaxChangeSpeed = 1;
    [SerializeField] GameObject m_followedObject = null;
    [SerializeField] float m_moveSpeed = 1;
    [SerializeField] float m_moveSpeedPow = 1;
    [SerializeField] float m_moveMaxSpeed = 100;
    [SerializeField] float m_rotationSpeed = 1;
    [SerializeField] float m_rotationSpeedPow = 1;

    Camera[] m_cameras;
    Vector2 m_oldTarget;

    float[] m_baseFOV;

    Rigidbody m_targetRigidbody;

    SubscriberList m_subscriberList = new SubscriberList();

    private void Awake()
    {
        m_subscriberList.Add(new Event<CameraInstantMoveEvent>.Subscriber(OnInstantMove));
        m_subscriberList.Subscribe();

        m_cameras = GetComponentsInChildren<Camera>();
        if (m_cameras.Length == 0)
        {
            Debug.LogError("You must have at least one camera");
            return;
        }

        m_baseFOV = new float[m_cameras.Length];
        for (int i = 0; i < m_cameras.Length; i++)
            m_baseFOV[i] = m_cameras[i].fieldOfView;

        if (m_followedObject != null)
            m_targetRigidbody = m_followedObject.GetComponent<Rigidbody>();
    }

    private void OnDestroy()
    {
        m_subscriberList.Unsubscribe();
    }
    
    void FixedUpdate()
    {
        if (m_cameras.Length == 0)
            return;
        if (m_followedObject == null)
            return;

        UpdateFOV();
        UpdatePosition();
        UpdateRotation();
    }

    void UpdateFOV()
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.z);
        Vector2 target = new Vector2(m_followedObject.transform.position.x, m_followedObject.transform.position.z);
        
        float distance = m_targetRigidbody != null ? m_targetRigidbody.velocity.magnitude : (target - m_oldTarget).magnitude / Time.deltaTime;
        float newZoom = m_fovOffsetOnMoveSpeed.Evaluate(distance);

        m_oldTarget = target;

        for (int i = 0; i < m_cameras.Length; i++)
        {
            float newFOV = newZoom + m_baseFOV[i];

            float delta = (newFOV - m_cameras[i].fieldOfView) / Time.deltaTime;

            if (Mathf.Abs(delta) > m_fovMaxChangeSpeed)
                delta = Mathf.Sign(delta) * m_fovMaxChangeSpeed;
            delta *= Time.deltaTime;

            m_cameras[i].fieldOfView += delta;
        }
    }

    void UpdatePosition()
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.z);
        Vector2 target = new Vector2(m_followedObject.transform.position.x, m_followedObject.transform.position.z);

        var dir = target - pos;
        var distance = dir.magnitude;

        float move = Mathf.Pow(distance * m_moveSpeed, m_moveSpeedPow);
        if (move > m_moveMaxSpeed)
            move = m_moveMaxSpeed;
        if(distance > 0.0001f)
        dir *= move * Time.deltaTime / distance;

        transform.position = transform.position + new Vector3(dir.x, 0, dir.y);
    }

    void UpdateRotation()
    {
        float rotTarget = m_followedObject.transform.rotation.eulerAngles.y;
        float rot = transform.rotation.eulerAngles.y - 90;

        float delta = MyMath.SignedDeltaAngle(rot, rotTarget);

        float move = Mathf.Sign(delta) * Mathf.Pow(Mathf.Abs(delta) * m_rotationSpeed, m_rotationSpeedPow);
        rot += move * Time.deltaTime;

        transform.rotation = Quaternion.Euler(90, rot + 90, 0);
    }

    void OnInstantMove(CameraInstantMoveEvent e)
    {
        transform.position = new Vector3(e.target.x, transform.position.y,  e.target.y);

        for (int i = 0; i < m_cameras.Length; i++)
            m_cameras[i].fieldOfView = m_baseFOV[i] + e.zoom;
    }
}
