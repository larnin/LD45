using UnityEngine;
using System.Collections;

public class DriftTrails : MonoBehaviour
{
    SubscriberList m_subscriberList = new SubscriberList();

    TrailRenderer[] m_trails;

    private void Awake()
    {
        m_trails = GetComponentsInChildren<TrailRenderer>();

        m_subscriberList.Add(new Event<EnableDriftingEvent>.Subscriber(OnDrift));
        m_subscriberList.Subscribe();

        foreach (var t in m_trails)
        {
            t.emitting = false;
        }
    }

    private void OnDestroy()
    {
        m_subscriberList.Unsubscribe();
    }

    void OnDrift(EnableDriftingEvent e)
    {
        foreach(var t in m_trails)
        {
            t.emitting = e.enabled;
        }
    }
}
