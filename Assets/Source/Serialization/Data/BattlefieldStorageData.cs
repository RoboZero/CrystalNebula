using System;
using System.Collections.Generic;

namespace Source.Serialization.Data
{
    [Serializable]
    public class BattlefieldStorageData
    {
        public int Length;
        public List<BattlefieldItemData> Items;
    }
}