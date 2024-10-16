using System;
using System.Collections.Generic;
using Source.Logic.State;

namespace Source.Logic.Data
{
    [Serializable]
    public class LineStorageData
    {
        public int Length;
        public List<LineItemData> Items;
    }
}