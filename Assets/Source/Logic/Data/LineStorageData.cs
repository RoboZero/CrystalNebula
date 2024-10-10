using System;
using Source.Logic.State;

namespace Source.Logic.Data
{
    [Serializable]
    public class LineStorageData
    {
        public int Length;
        public LineItemData[] Items;
    }
}