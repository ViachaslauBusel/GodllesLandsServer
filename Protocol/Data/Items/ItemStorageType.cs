using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Data.Items
{
    public enum ItemStorageType: byte
    {
        None,
        Equipment = 1,
        PrimaryBag = 2,
        SecondaryBag = 3,
        SmelerComponent = 4,
        SmelterFuel = 5,
    }
}
