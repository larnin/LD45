using UnityEngine;
using System.Collections;

public class UpgradePicker : MonoBehaviour
{
    [SerializeField] LayerMask m_upgradeMask;

    private void OnTriggerEnter(Collider other)
    {
        if ((m_upgradeMask.value & (1 << other.gameObject.layer)) != 0)
        {
            Event<PickUpgradeEvent>.Broadcast(new PickUpgradeEvent());
            Destroy(other.gameObject);
        }
    }
}
