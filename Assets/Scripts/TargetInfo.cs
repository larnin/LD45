using UnityEngine;
using System.Collections;

public class TargetInfo : MonoBehaviour
{
    void Start()
    {
        Event<RegisterTargetEvent>.Broadcast(new RegisterTargetEvent(this));
    }
}
