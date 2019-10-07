using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NRand;
using DG.Tweening;

public class TargetManager : MonoBehaviour
{
    [SerializeField] int m_noUseOldCount = 1;
    [SerializeField] GameObject m_targetPrefab = null;
    [SerializeField] float m_minTargetDistance = 10;

    List<TargetInfo> m_targets = new List<TargetInfo>();

    SubscriberList m_subscriberList = new SubscriberList();

    int[] m_lastUsedList;

    private void Awake()
    {
        m_subscriberList.Add(new Event<RegisterTargetEvent>.Subscriber(RegisterTarget));
        m_subscriberList.Add(new Event<GenerateTargetEvent>.Subscriber(GenerateTarget));
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

    public int GetTargetCount() {
        return m_targets.Count;
    }

    void GenerateTarget(GenerateTargetEvent e)
    {
        GenerateTarget(false);
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
            
            if(m_noUseOldCount > 0 && m_lastUsedList[0] != -1)
            {
                var current = new Vector2(m_targets[m_lastUsedList[0]].transform.position.x, m_targets[m_lastUsedList[0]].transform.position.y);
                var next = new Vector2(m_targets[m_lastUsedList[0]].transform.position.x, m_targets[m_lastUsedList[0]].transform.position.y);

                if ((current - next).magnitude < m_minTargetDistance)
                    continue;
            }
        }

        if (m_noUseOldCount > 0)
        {
            for (int i = 0; i < m_noUseOldCount - 1; i++)
                m_lastUsedList[i + 1] = m_lastUsedList[i];
            m_lastUsedList[0] = index;
        }

        var obj = Instantiate(m_targetPrefab);

        var pos = m_targets[index].transform.position;
        float dist = 0;
        if (CarControler.instance != null)
            dist = (pos - CarControler.instance.transform.position).magnitude;

        obj.transform.position = pos;

        Event<UpdateTargetPositionEvent>.Broadcast(new UpdateTargetPositionEvent(pos, dist, firstTime));
    }
}
