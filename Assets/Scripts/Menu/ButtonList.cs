using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonList : MonoBehaviour
{
    const string verticalAxis = "Vertical";
    const string pressButton = "Submit";

    [SerializeField] List<ButtonLogic> m_buttons = new List<ButtonLogic>();
    //[SerializeField] AudioClip m_moveSound = null;
    //[SerializeField] AudioClip m_pressSound = null;

    int m_currentIndex = -1;

    float m_oldAxisValue = 0;

    private void Start()
    {
        for (int i = 0; i < m_buttons.Count; i++)
            m_buttons[i].SetButtonList(this);
    }

    void Update()
    {
        float axisValue = Input.GetAxisRaw(verticalAxis);

        if (Mathf.Abs(m_oldAxisValue) < 0.5f && Mathf.Abs(axisValue) >= 0.5f)
        {
            int index = 0;
            if (m_currentIndex >= 0)
            {
                int offset = axisValue < 0 ? 1 : -1;
                index = m_currentIndex + offset;
                if (index < 0 || index >= m_buttons.Count)
                    index = m_currentIndex;
            }

            SetIndex(index);
        }

        if (Input.GetButtonDown(pressButton) && m_currentIndex >= 0)
            Press(m_buttons[m_currentIndex]);

        m_oldAxisValue = axisValue;
    }

    public void Hover(ButtonLogic button)
    {
        int index = ButtonIndex(button);
        if (index < 0)
            return;

        SetIndex(index);
    }

    public void Press(ButtonLogic button)
    {
        int index = ButtonIndex(button);
        if (index < 0)
            return;

        SetIndex(index);
        button.SetPressed();

        Event<ButtonPressEvent>.Broadcast(new ButtonPressEvent(index));

        //SoundSystem.Instance().play(m_pressSound);
    }

    public void Idle(ButtonLogic button)
    {
        int index = ButtonIndex(button);
        if (index < 0)
            return;

        if (m_currentIndex == index)
            SetIndex(-1);
    }

    int ButtonIndex(ButtonLogic button)
    {
        return m_buttons.IndexOf(button);
    }

    void SetIndex(int index)
    {
        if (m_currentIndex >= 0)
            m_buttons[m_currentIndex].SetIdle();

        m_currentIndex = index;

        if (m_currentIndex >= 0)
        {
            m_buttons[m_currentIndex].SetHovered();
            //SoundSystem.Instance().play(m_moveSound, 0.5f, true);
        }
    }

    public void Remove(ButtonLogic button)
    {
        int index = ButtonIndex(button);
        if (index < 0)
            return;

        if (m_currentIndex == index)
            SetIndex(-1);

        m_buttons.Remove(button);
    }
}
