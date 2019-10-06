using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] float m_initialTime = 60;
    [SerializeField] float m_timePerUnit = 1;
    [SerializeField] float m_timePerObjective = 5;
    [SerializeField] float m_timeMultiplierPerObjective = 0.9f;
    [SerializeField] Text m_text = null;

    float m_remainingTime = 0;
    int m_completedObjectives = 0;
    bool m_timeoutFired = false;

    SubscriberList m_subscriberList = new SubscriberList();

    private void Awake()
    {
        m_remainingTime = m_initialTime;

        m_subscriberList.Add(new Event<UpdateTargetPositionEvent>.Subscriber(OnNewObjective));
        m_subscriberList.Subscribe();
    }

    private void OnDestroy()
    {
        m_subscriberList.Unsubscribe();
    }
    

    // Update is called once per frame
    void Update()
    {
        int sec = Mathf.CeilToInt(m_remainingTime);
        int min = sec / 60;
        sec %= 60;

        string text = "";
        if (min > 0)
            text += min.ToString() + ":";
        if (sec < 10)
            text += "0";
        text += sec.ToString();

        m_text.text = text;

        m_remainingTime -= Time.deltaTime;

        if(m_remainingTime <= 0 && !m_timeoutFired)
        {
            Event<TimeoutEvent>.Broadcast(new TimeoutEvent());
            m_timeoutFired = true;
        }
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
}
