using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler {
    [SerializeField] AudioClip m_clickSound;
    [SerializeField] AudioClip m_hoverSound;

    private AudioSource m_audioSource;

    // Start is called before the first frame update
    void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
        GetComponent<Button>().onClick.AddListener(Click);
    }

    void Click() {
        m_audioSource.PlayOneShot(m_clickSound);
    }

    public void OnPointerEnter(PointerEventData pointerEventData) {
        m_audioSource.PlayOneShot(m_hoverSound);
    }

}
