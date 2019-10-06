using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ButtonLogic : MonoBehaviour
{
    [SerializeField] GameObject m_idleObject = null;
    [SerializeField] GameObject m_hoveredObject = null;
    [SerializeField] GameObject m_pressedObject = null;
    [SerializeField] float m_pressDuration = 0.5f;

    bool m_hovered = false;
    ButtonList m_buttonList = null;

    private void Start()
    {
        SetIdle();
    }

    private void OnDestroy()
    {
        if (m_buttonList != null)
            m_buttonList.Remove(this);
    }

    private void OnMouseEnter()
    {
        m_buttonList.Hover(this);
    }

    private void OnMouseExit()
    {
        m_buttonList.Idle(this);
    }

    private void OnMouseDown()
    {
        m_buttonList.Press(this);
    }

    public void SetButtonList(ButtonList list)
    {
        m_buttonList = list;
    }

    public void SetHovered()
    {
        m_idleObject.SetActive(false);
        m_pressedObject.SetActive(false);
        m_hoveredObject.SetActive(true);
        m_hovered = true;
    }

    public void SetPressed()
    {
        m_idleObject.SetActive(false);
        m_pressedObject.SetActive(true);
        m_hoveredObject.SetActive(false);
        m_hovered = true;

        DOVirtual.DelayedCall(m_pressDuration, () =>
        {
            if (this == null)
                return;
            if (!m_hovered)
                return;
            SetHovered();
        });
    }

    public void SetIdle()
    {
        m_idleObject.SetActive(true);
        m_pressedObject.SetActive(false);
        m_hoveredObject.SetActive(false);
        m_hovered = false;
    }
}
