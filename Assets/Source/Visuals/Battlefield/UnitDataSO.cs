using Source.Logic.Data;
using Source.Utility;
using UnityEngine;

namespace Source.Visuals.Battlefield
{
    [CreateAssetMenu(fileName = "UnitName", menuName = "Game/Unit")]
    public class UnitDataSO : DescriptionBaseSO
    {
        public Sprite Sprite;
        public string Name;
        public int BaseHealth = 3;
        public int BasePower = 1;

        public UnitData CreateDefault(int ownerId, string definition)
        {
            return new UnitData()
            {
                Definition = definition,
                OwnerId = ownerId,
                Health = BaseHealth,
                Power = BasePower
            };
        }
    }
}
