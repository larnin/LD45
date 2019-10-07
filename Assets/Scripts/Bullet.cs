using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject m_explosionPrefab = null;
    [SerializeField] float m_damages = 0;
    [SerializeField] float m_destroyTime = -1;

    void Start()
    {
        if (m_destroyTime > 0)
            Destroy(gameObject, m_destroyTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        Damage(collision.collider, true);
    }

    void OnTriggerEnter(Collider c)
    {
        Damage(c, false);
    }

    void Damage(Collider c, bool destroy)
    {
        if (m_explosionPrefab != null)
        {
            var obj = Instantiate(m_explosionPrefab);
            obj.transform.position = transform.position;
        }

        var life = c.GetComponent<Life>();
        if(life != null)
        {
            life.Damage(m_damages);
        }

        if (destroy)
            Destroy(gameObject);
    }
}