using System;

namespace Source.Serialization.Data
{
    [Serializable]
    public class BattlefieldItemData
    {
        public int Location;
        public BuildingData Building;
        public UnitData Unit;
    }
}