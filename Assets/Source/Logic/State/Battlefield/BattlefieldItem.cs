using System;
using Source.Logic.State.LineItems.Units;

namespace Source.Logic.State.Battlefield
{
    // TODO: A Line item shouldn't contain other line items. 
    [Serializable]
    public class BattlefieldItem : LineItem
    {
        public int DeploymentZoneOwnerId;
        public BuildingMemory Building;
        public UnitMemory Unit;
    }
}