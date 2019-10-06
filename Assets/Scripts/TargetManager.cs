using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NRand;
using DG.Tweening;

public class TargetManager : MonoBehaviour
{
    [SerializeField] int m_noUseOldCount;
    [SerializeField] GameObject m_targetPrefab;

    List<TargetInfo> m_targets = new List<TargetInfo>();

    SubscriberList m_subscriberList = new SubscriberList();

    int[] m_lastUsedList;

    private void Awake()
    {
        m_subscriberList.Add(new Event<RegisterTargetEvent>.Subscriber(RegisterTarget));
        m_subscriberList.Subscribe();

        m_lastUsedList = new int[m_noUseOldCount];
        for (int i = 0; i < m_noUseOldCount; i++)
            m_lastUsedList[i] = -1;
    }

    private void OnDestroy()
    {
        m_subscriberList.Unsubscribe();
    }

    private void Start()
    {
        DOVirtual.DelayedCall(0.1f, () => GenerateTarget(true));
    }

    void RegisterTarget(RegisterTargetEvent e)
    {
        m_targets.Add(e.target);
    }

    void GenerateTarget(bool firstTime)
    {
        int index = 0;
        for(int loop = 0; loop < 10; loop++)
        {
            index = new UniformIntDistribution(0, m_targets.Count - 1).Next(new StaticRandomGenerator<MT19937>());

            bool onList = false;
            for(int i = 0; i < m_noUseOldCount; i++)
            {
                if(index == m_lastUsedList[i])
                {
                    onList = true;
                    break;
                }
            }

            if (onList)
                continue;
        }

        for (int i = 0; i < m_noUseOldCount - 1; i++)
            m_lastUsedList[i + 1] = m_lastUsedList[i];
        m_lastUsedList[0] = index;

        var obj = Instantiate(m_targetPrefab);

        var pos = m_targets[index].transform.position;
        float dist = 0;
        if (CarControler.instance != null)
            dist = (pos - CarControler.instance.transform.position).magnitude;

        obj.transform.position = pos;

        Event<UpdateTargetPositionEvent>.Broadcast(new UpdateTargetPositionEvent(pos, dist, firstTime));
    }
}
