using UnityEngine;
using System.Collections;

public class PauseMenuLogic : MonoBehaviour
{
    [SerializeField] string m_mainMenuName = "MainMenu";

    SubscriberList m_subscriberList = new SubscriberList();

    private void Awake()
    {
        m_subscriberList.Add(new Event<ButtonPressEvent>.Subscriber(OnMenuSelect));
        m_subscriberList.Subscribe();

        Time.timeScale = 0;
    }

    private void OnDestroy()
    {
        m_subscriberList.Unsubscribe();

        Time.timeScale = 1;
    }

    void OnMenuSelect(ButtonPressEvent e)
    {
        switch (e.index)
        {
            case 0:
                Time.timeScale = 1;
                Destroy(gameObject);
                break;
            case 1:
                SceneSystem.changeScene(m_mainMenuName);
                break;
            default:
                Debug.LogError("Help me please, i don't know what to do with that button :'(");
                break;
        }
    }
}
