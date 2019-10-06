using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ButtonPressEvent : EventArgs
{
    public ButtonPressEvent(int _index)
    {
        index = _index;
    }

    public int index;
}
