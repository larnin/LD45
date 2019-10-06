using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BroadcastUpgradeEvent : EventArgs
{
    public BroadcastUpgradeEvent(UpgradeManager.Upgrade _upgrade)
    {
        upgrade = _upgrade;
    }

    public UpgradeManager.Upgrade upgrade;
}
