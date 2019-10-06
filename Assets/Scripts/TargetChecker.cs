using UnityEngine;
using System.Collections;

public class TargetChecker : MonoBehaviour
{
    [SerializeField] LayerMask m_targetLayer;

    private void OnTriggerEnter(Collider other)
    {
        if((m_targetLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            Transform tr = other.transform;
            while (tr.parent != null)
                tr = tr.parent;
            Destroy(tr.gameObject);
            Event<GenerateTargetEvent>.Broadcast(new GenerateTargetEvent());
        }
    }
}
