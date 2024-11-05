using System;

namespace Source.Logic.State
{
    [Serializable]
    public class BattlefieldItem : DataItem
    {
        public Building Building;
        public Unit Unit;
    }
}