using System;
using System.Collections.Generic;
using System.Text;

namespace Godless_Lands_Game.Characteristics
{
   public class Stats
    {
        public string Name { get; protected set; }
        public int MinPattack { get; protected set; }
        public int MaxPattack { get; protected set; }
        public int PhysicalDefense { get; protected set; }
        public float AttackSpeed { get; protected set; }
        public float MoveSpeed { get; protected set; }
    }
}
