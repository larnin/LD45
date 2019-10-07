using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] float m_initialTime = 60;
    [SerializeField] float m_timePerUnit = 1;
    [SerializeField] float m_timePerObjective = 5;
    [SerializeField] float m_timeMultiplierPerObjective = 0.9f;
    [SerializeField] TMP_Text m_text = null;

    float m_remainingTime = 0;
    int m_completedObjectives = 0;
    bool m_timeoutFired = false;

    SubscriberList m_subscriberList = new SubscriberList();

    private void Awake()
    {
        m_remainingTime = m_initialTime;

        m_subscriberList.Add(new Event<UpdateTargetPositionEvent>.Subscriber(OnNewObjective));
        m_subscriberList.Add(new Event<AddTimeEvent>.Subscriber(OnAddTime));
        m_subscriberList.Add(new Event<EnableTimerEvent>.Subscriber(OnEnableTimer));
        m_subscriberList.Subscribe();
    }

    private void OnDestroy()
    {
        m_subscriberList.Unsubscribe();
    }
    

    // Update is called once per frame
    void Update()
    {
        int sec = Mathf.FloorToInt(m_remainingTime);
        int ms = Mathf.FloorToInt((m_remainingTime - sec) * 100);
        int min = sec / 60;
        sec %= 60;
        
        string text = "";
        text += min.ToString("00") + ":";
        text += sec.ToString("00") + ":";
        text += ms.ToString("00");

        m_text.text = text;

        m_remainingTime -= Time.deltaTime;

        if(m_remainingTime <= 0 && !m_timeoutFired)
        {
            Event<TimeoutEvent>.Broadcast(new TimeoutEvent());
            m_timeoutFired = true;
        }

        if (m_remainingTime < 0)
            m_remainingTime = 0;
    }

    void OnNewObjective(UpdateTargetPositionEvent e)
    {
        if (e.firstObjective)
            return;

        float time = m_timePerUnit * e.distance + m_timePerObjective;

        time = time * MyMath.powInt(m_timeMultiplierPerObjective, m_completedObjectives);

        m_completedObjectives++;

        m_remainingTime += time;
    }

    void OnAddTime(AddTimeEvent e)
    {
        m_remainingTime += e.time;
    }

    void OnEnableTimer(EnableTimerEvent e)
    {
        gameObject.SetActive(e.enable);
    }

    public int GetTargetCount()
    {
        return m_completedObjectives;
    }
}
