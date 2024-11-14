using System;
using System.Collections.Generic;

namespace Source.Visuals.Tooltip
{
    [Serializable]
    public class TooltipContent
    {
        public string Header;
        public List<Stat> Stats = new();
        public string Description;

        public struct Stat
        {
            public string Name;
            public string Value;
        }

        public void Clear()
        {
            Header = "";
            Stats.Clear();
            Description = "";
        }
    }
}