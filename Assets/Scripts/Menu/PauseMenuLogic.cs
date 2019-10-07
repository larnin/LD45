using UnityEngine;
using System.Collections;

public class PauseMenuLogic : MonoBehaviour
{
    [SerializeField] string m_mainMenuName = "MainMenu";


    private void Awake()
    {
        Time.timeScale = 0;
    }

    private void OnDestroy()
    {

        Time.timeScale = 1;
    }

    public void SelectMenu(int index)
    {
        switch (index)
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
