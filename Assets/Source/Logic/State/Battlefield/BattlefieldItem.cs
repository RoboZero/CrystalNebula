using System;

namespace Source.Logic.State.Battlefield
{
    [Serializable]
    public class BattlefieldItem : DataItem
    {
        public Building Building;
        public Unit Unit;
    }
}