using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMenuSound : MonoBehaviour
{
    AudioSource m_audiosource;

    void Awake()
    {
        m_audiosource = GetComponent<AudioSource>();
    }

    public void PlayWoosh() {
        m_audiosource.Play();
    }
}
