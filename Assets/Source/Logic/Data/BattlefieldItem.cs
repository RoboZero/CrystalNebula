using System;

namespace Source.Logic.Data
{
    [Serializable]
    public class BattlefieldItem : DataItem
    {
        public Building Building;
        public Unit Unit;
    }
}