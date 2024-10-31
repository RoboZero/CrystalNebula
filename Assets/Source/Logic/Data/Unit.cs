using System;

namespace Source.Logic.Data
{
    [Serializable]
    public class Unit : BattlefieldResource
    {
        public int Health;
        public int Power;
        public bool canSwitchPlaces;
        public bool canEngageCombat;

        public override string ToString()
        {
            return $"{Definition}: (OId: {OwnerId} H {Health}, P {Power})";
        }
    }
}
