using System;
using System.Collections.Generic;

namespace Source.Logic.Data
{
    [Serializable]
    public class BattlefieldStorageData
    {
        public int Length;
        public List<BattlefieldItemData> Items;
    }
}