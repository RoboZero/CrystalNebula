using System;
using Source.Serialization.Data;

namespace Source.Logic
{
    [Serializable]
    public class Unit : BattlefieldResource
    {
        public int Health;
        public int Power;

        public override string ToString()
        {
            return $"{Definition}: (OId: {OwnerId} H {Health}, P {Power})";
        }
    }
}
