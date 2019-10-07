using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitterMenu : MonoBehaviour
{
    [SerializeField] GameObject[] m_textArray;
    GameObject m_currentText;
    Animator m_animator;


    // Start is called before the first frame update
    void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    private void Start() {
        StartCoroutine(TwitterCoroutine());
    }

    IEnumerator TwitterCoroutine() {
        yield return new WaitForSeconds(0.5f);

        foreach(GameObject vText in m_textArray) {
            if(m_currentText != null) {
                m_currentText.SetActive(false);
            }
            vText.SetActive(true);
            m_currentText = vText;

            yield return new WaitForSeconds(5);
        }

        m_animator.SetTrigger("Close");

        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

}
