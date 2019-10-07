using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenuTrigger : MonoBehaviour
{
    [SerializeField] private GameObject m_gameOverPrefab;

    SubscriberList m_subscriberList = new SubscriberList();

    GameObject m_gameOverObject = null;

    private void Awake() 
    {
        m_subscriberList.Add(new Event<TimeoutEvent>.Subscriber(Timeout));
        m_subscriberList.Subscribe();
    }

    private void OnDestroy() {
        m_subscriberList.Unsubscribe();
    }

    private void Timeout(TimeoutEvent e) 
    {
        if(m_gameOverObject == null) 
        {
            Instantiate(m_gameOverPrefab);
        }
    }

}
