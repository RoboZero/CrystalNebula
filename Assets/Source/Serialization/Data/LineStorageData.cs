using System;
using System.Collections.Generic;

namespace Source.Serialization.Data
{
    [Serializable]
    public class LineStorageData
    {
        public int Length;
        public List<LineItemData> Items;
    }
}