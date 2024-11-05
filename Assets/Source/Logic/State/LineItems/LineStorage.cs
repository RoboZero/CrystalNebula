using System;
using System.Collections.Generic;

namespace Source.Logic.State.LineItems
{
    [Serializable]
    public class LineStorage
    {
        public int Length;
        public List<LineItem> Items;
    }
}