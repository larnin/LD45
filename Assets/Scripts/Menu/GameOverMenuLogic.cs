﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenuLogic : MonoBehaviour
{
    [SerializeField] string m_mainMenuName = "MainMenu";
    [SerializeField] string m_gameName = "SampleScene";
    public void SelectMenu(int index) {
        switch (index) {
            case 0:
                SceneSystem.changeScene(m_gameName);
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
