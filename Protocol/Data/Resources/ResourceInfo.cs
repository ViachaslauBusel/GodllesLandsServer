using Protocol.Data.Items;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Protocol.Data.Resources
{
    public class ResourceInfo
    {
        public int ID { get; set; }
        public int Profesion { get; set; }
        public int Exp { get; set; }
        public int Stamina { get; set; }
        public List<DropItemData> Drops { get; set; }
    }
}
