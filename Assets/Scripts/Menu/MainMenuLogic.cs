using UnityEngine;
using System.Collections;

public class MainMenuLogic : MonoBehaviour
{
    [SerializeField] string m_sceneName = "";

    public void SelectMenu(int index)
    {
        switch(index)
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
