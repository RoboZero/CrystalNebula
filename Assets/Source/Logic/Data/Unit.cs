using System;

namespace Source.Logic.Data
{
    [Serializable]
    public class Unit : BattlefieldResource
    {
        public int Health;
        public int Power;
        public bool CanSwitchPlaces;
        public bool CanEngageCombat;

        public override string ToString()
        {
            return $"{Definition}: (OId: {OwnerId} H {Health}, P {Power})";
        }
    }
}
