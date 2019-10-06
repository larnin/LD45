using UnityEngine;
using System.Collections;

public class PauseMenuTrigger : MonoBehaviour
{
    string pauseName = "Pause";

    [SerializeField] GameObject m_pausePrefab = null;

    GameObject m_pauseObject = null;

    void Update()
    {
        if(Input.GetButtonDown(pauseName) && m_pauseObject == null)
        {
            m_pauseObject = Instantiate(m_pausePrefab);
        }
    }
}
