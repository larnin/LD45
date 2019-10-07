using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class EnemyLife : Life
{
    [SerializeField] float m_maxLife = 5;
    [SerializeField] float m_whiteTimeOnDamage = 0.2f;
    [SerializeField] GameObject m_deathObject = null;

    float m_life;

    void Start()
    {
        m_life = m_maxLife;
    }

    public override void Damage(float value)
    {
        m_life -= value;

        if(m_life <= 0)
        {
            OnDeath();
            return;
        }

        var renderers = GetComponentsInChildren<Renderer>();

        foreach (var r in renderers)
            foreach (var m in r.materials)
                m.SetColor("_AdditiveColor", Color.white);

        DOVirtual.DelayedCall(m_whiteTimeOnDamage, () =>
        {
            foreach (var r in renderers)
            {
                if (r != null)
                {
                    foreach (var m in r.materials)
                        m.SetColor("_AdditiveColor", Color.black);
                }
            }
        });
    }

    void OnDeath()
    {
        if (m_deathObject != null)
        {
            var obj = Instantiate(m_deathObject);
            obj.transform.position = transform.position;
        }

        Destroy(gameObject);
    }
}
