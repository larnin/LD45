using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerLife : Life
{
    [SerializeField] float m_damageTakenMultiplier = 1;
    [SerializeField] GameObject m_explosionOnTimeout = null;
    [SerializeField] float m_damageSpeedHit = 1;

    SubscriberList m_subscriberList = new SubscriberList();

    void Awake()
    {
        m_subscriberList.Add(new Event<TimeoutEvent>.Subscriber(OnTimeout));
        m_subscriberList.Subscribe();
    }

    void OnDestroy()
    {
        m_subscriberList.Unsubscribe();
    }

    public override void Damage(float value)
    {
        Event<AddTimeEvent>.Broadcast(new AddTimeEvent(-value * m_damageTakenMultiplier));
    }

    void OnTimeout(TimeoutEvent e)
    {
        var obj = Instantiate(m_explosionOnTimeout);
        obj.transform.position = transform.position;

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var rigidbody = GetComponent<Rigidbody>();
        if (rigidbody == null)
            return;

        var life = collision.collider.GetComponent<Life>();
        if(life != null)
        {
            //life.Damage(rigidbody.velocity.magnitude * m_damageSpeedHit);
        }
        else
        {
            //play hit sound
        }
    }
}
