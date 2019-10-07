using UnityEngine;
using System.Collections;

public class CursorLogic : MonoBehaviour
{
    SubscriberList m_subscriberList = new SubscriberList();
    SpriteRenderer m_renderer = null;

    private void Awake()
    {
        m_subscriberList.Add(new Event<WeaponTargetChangeEvent>.Subscriber(OnCursorMove));
        m_subscriberList.Subscribe();

        m_renderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnDestroy()
    {
        m_subscriberList.Unsubscribe();
    }

    void OnCursorMove(WeaponTargetChangeEvent e)
    {
        transform.position = new Vector3(e.target.x, e.target.y, transform.position.z);
        Color c = Color.white;
        if (e.distance < 1)
            c.a *= e.distance;
        m_renderer.color = c;
    }
}
