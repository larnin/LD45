using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NRand;

public class DriftTrails : MonoBehaviour
{
    [SerializeField] List<AudioClip> m_clips = new List<AudioClip>();

    SubscriberList m_subscriberList = new SubscriberList();

    TrailRenderer[] m_trails;
    AudioSource m_source;

    private void Awake()
    {
        m_trails = GetComponentsInChildren<TrailRenderer>();

        m_subscriberList.Add(new Event<EnableDriftingEvent>.Subscriber(OnDrift));
        m_subscriberList.Subscribe();

        foreach (var t in m_trails)
        {
            t.emitting = false;
        }

        m_source = GetComponent<AudioSource>();
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

        if(e.enabled)
        {
            m_source.clip = m_clips[new UniformIntDistribution(0, m_clips.Count - 1).Next(new StaticRandomGenerator<MT19937>())];
            m_source.time = 0;
            m_source.Play();
        }
    }
}
