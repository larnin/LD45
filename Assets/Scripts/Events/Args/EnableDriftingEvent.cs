using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class EnableDriftingEvent : EventArgs
{
    public EnableDriftingEvent(bool _enabled)
    {
        enabled = _enabled;
    }

    public bool enabled;
}