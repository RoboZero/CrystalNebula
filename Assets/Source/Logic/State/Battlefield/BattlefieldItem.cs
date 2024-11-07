using System;
using Source.Logic.State.LineItems.Units;

namespace Source.Logic.State.Battlefield
{
    [Serializable]
    public class BattlefieldItem : LineItem
    {
        public BuildingMemory Building;
        public UnitMemory Unit;
    }
}