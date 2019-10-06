using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RegisterTargetEvent : EventArgs
{
    public RegisterTargetEvent(TargetInfo _target)
    {
        target = _target;
    }

    public TargetInfo target;
}