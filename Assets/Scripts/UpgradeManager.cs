using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    [Serializable]
    public class Upgrade
    {
        public GameObject m_prefab;
        public float m_forwardSpeed;
        public float m_backwardSpeep;
        public float m_acceleration;
        public float m_passiveDeceleration;
        public float m_frontWheelRotation;
        public float m_wheelDistance;
        public float m_maxRotSpeedBeforeDrift;
        public float m_driftDeceleration;
        public float m_driftLostMaxSpeed;
    }

    [SerializeField] List<Upgrade> m_upgrades = new List<Upgrade>();

    SubscriberList m_subscriberList = new SubscriberList();

    int m_nCurrentUpgrade = 0;

    private void Awake()
    {
        m_subscriberList.Add(new Event<PickUpgradeEvent>.Subscriber(OnUpgradePick));
        m_subscriberList.Subscribe();
    }

    private void OnDestroy()
    {
        m_subscriberList.Unsubscribe();
    }

    void OnUpgradePick(PickUpgradeEvent e)
    {
        if (m_nCurrentUpgrade >= m_upgrades.Count)
            return;

        Event<BroadcastUpgradeEvent>.Broadcast(new BroadcastUpgradeEvent(m_upgrades[m_nCurrentUpgrade]));

        m_nCurrentUpgrade++;
    }
}
