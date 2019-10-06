using UnityEngine;
using System.Collections;

public class CarRenderUpdater : MonoBehaviour
{
    SubscriberList m_subscriberList = new SubscriberList();

    private void Awake()
    {
        m_subscriberList.Add(new Event<BroadcastUpgradeEvent>.Subscriber(OnUpgrade));
        m_subscriberList.Subscribe();
    }

    private void OnDestroy()
    {
        m_subscriberList.Unsubscribe();
    }

    void OnUpgrade(BroadcastUpgradeEvent e)
    {
        for(int i = transform.childCount - 1; i >= 0; i-- )
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        var obj = Instantiate(e.upgrade.m_prefab);
        obj.transform.parent = transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.Euler(0, -90, 90);
    }
}
