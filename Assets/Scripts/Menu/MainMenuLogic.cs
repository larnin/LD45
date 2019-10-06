using UnityEngine;
using System.Collections;

public class MainMenuLogic : MonoBehaviour
{
    [SerializeField] string m_sceneName = "";

    SubscriberList m_subscriberList = new SubscriberList();

    private void Awake()
    {
        m_subscriberList.Add(new Event<ButtonPressEvent>.Subscriber(OnMenuSelect));
        m_subscriberList.Subscribe();
    }

    private void OnDestroy()
    {
        m_subscriberList.Unsubscribe();
    }

    void OnMenuSelect(ButtonPressEvent e)
    {
        switch(e.index)
        {
            case 0:
                SceneSystem.changeScene(m_sceneName);
                break;
            case 1:
                Application.Quit();
                break;
            default:
                Debug.LogError("Help me please, i don't know what to do with that button :'(");
                break;
        }
    }
}
