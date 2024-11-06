using Source.Logic;
using Source.Logic.State;
using Source.Serialization.Data;
using Source.Utility;
using UnityEngine;

namespace Source.Visuals.Battlefield
{
    [CreateAssetMenu(fileName = "BuildingName", menuName = "Game/Building")]
    public class BuildingDataSO : DescriptionBaseSO
    {
        public Sprite Sprite;
        public string Name;
        public string Abbreviation;
        public int BaseHealth;
        public int BasePower;

        public Building CreateInstance(BuildingData buildingData)
        {
            return new Building()
            {
                OwnerId = buildingData.OwnerId,
                Definition = buildingData.Definition,
                Health = buildingData.Health,
                Power = buildingData.Power
            };
        }

        public Building CreateDefault(int ownerId, string definition, int? health = null, int? power = null)
        {
            return new Building()
            {
                OwnerId = ownerId,
                Definition = definition,
                Health = health ?? BaseHealth,
                Power = power ?? BasePower
            };
        }
    }
}
