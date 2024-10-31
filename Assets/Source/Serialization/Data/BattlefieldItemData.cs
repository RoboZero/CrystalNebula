using System;
using Source.Logic;
using Source.Logic.Data;

namespace Source.Serialization.Data
{
    [Serializable]
    public class BattlefieldItemData : DataItem
    {
        public int Location;
        public BuildingData Building;
        public UnitData Unit;
    }
}