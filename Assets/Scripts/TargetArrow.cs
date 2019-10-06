using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TargetArrow : MonoBehaviour
{
    [SerializeField] float m_border = 20;
    [SerializeField] float m_hideMinDistance = 5;
    [SerializeField] float m_hideMaxDistance = 20;

    SubscriberList m_subscriberList = new SubscriberList();

    Vector2 m_target;

    RectTransform m_canvasTransform;

    Graphic[] m_renderers;

    private void Awake()
    {
        m_subscriberList.Add(new Event<UpdateTargetPositionEvent>.Subscriber(OnTargetChange));
        m_subscriberList.Subscribe();

        var canvas = transform.GetComponentInParent<Canvas>();
        if (canvas != null)
            m_canvasTransform = canvas.GetComponent<RectTransform>();

        m_renderers = GetComponentsInChildren<Graphic>();
    }

    private void OnDestroy()
    {
        m_subscriberList.Unsubscribe();
    }

    void Update()
    {
        if (m_canvasTransform == null)
            return;

        var player = CarControler.instance;

        if (player == null)
            return;

        var playerPos = new Vector2(player.transform.position.x, player.transform.position.z);

        var bounds = m_canvasTransform.rect;
        bounds.width -= 2 * m_border;
        bounds.height -= 2 * m_border;

        float alpha = (playerPos - m_target).magnitude;
        var dir = -(playerPos - m_target) / Mathf.Max(0.001f, alpha);

        if (alpha < m_hideMinDistance)
            alpha = 0;
        else if (alpha > m_hideMaxDistance)
            alpha = 1;
        else alpha = (alpha - m_hideMinDistance) / (m_hideMaxDistance - m_hideMinDistance);
        foreach (var r in m_renderers)
        {
            Color c = r.color;
            c.a = alpha;
            r.color = c;
        }
            
        var pos = dir * (bounds.width + bounds.height) / 2.0f;

        if(Mathf.Abs(pos.x) > bounds.width / 2)
            pos *= bounds.width / Mathf.Abs(pos.x) / 2;
        if (Mathf.Abs(pos.y) > bounds.height / 2)
            pos.y *= bounds.height / Mathf.Abs(pos.y) / 2;

        pos += new Vector2(m_border, m_border) + new Vector2(bounds.width, bounds.height) / 2.0f;

        float rot = Mathf.Rad2Deg * Mathf.Atan2(dir.y, dir.x);

        transform.position = new Vector3(pos.x, pos.y, 0);
        transform.rotation = Quaternion.Euler(0, 0, rot);
    }

    void OnTargetChange(UpdateTargetPositionEvent e)
    {
        m_target = new Vector2(e.position.x, e.position.z);
    }
}
