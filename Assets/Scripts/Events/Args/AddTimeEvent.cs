using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AddTimeEvent : EventArgs
{
    public AddTimeEvent(float _time)
    {
        time = _time;
    }

    public float time;
}
