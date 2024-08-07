﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Data.Items
{
    public class ElixirItemInfo : ItemInfo
    {
        public readonly int RestoreHP;
        public readonly int RestoreMP;
        public readonly int RestoreStamina;

        public ElixirItemInfo(int id, bool isStackable, int weight, int restoreHP, int restoreMP, int restoreStamina)
            : base(id, isStackable, weight)
        {
            RestoreHP = restoreHP;
            RestoreMP = restoreMP;
            RestoreStamina = restoreStamina;
        }
    }
}
