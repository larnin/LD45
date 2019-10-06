using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UpdateTargetPositionEvent : EventArgs
{
    public UpdateTargetPositionEvent(Vector3 _position, float _distance, bool _firstObjective)
    {
        position = _position;
        distance = _distance;
        firstObjective = _firstObjective;
    }

    public Vector3 position;
    public float distance;
    public bool firstObjective;
}