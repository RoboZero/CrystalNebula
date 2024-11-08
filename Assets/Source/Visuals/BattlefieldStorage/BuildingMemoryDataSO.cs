using Source.Logic.State.Battlefield;
using Source.Logic.State.LineItems.Units;
using Source.Serialization.Data;
using Source.Utility;
using UnityEngine;

namespace Source.Visuals.BattlefieldStorage
{
    [CreateAssetMenu(fileName = "BuildingName", menuName = "Game/Building")]
    public class BuildingMemoryDataSO : DescriptionBaseSO
    {
        public Sprite Sprite;
        public string Name;
        public string Abbreviation;
        public int BaseHealth;
        public int BasePower;
        public float DataSize;

        public BuildingMemory CreateInstance(BuildingData buildingData)
        {
            return new BuildingMemory()
            {
                OwnerId = buildingData.OwnerId,
                Definition = buildingData.Definition,
                Health = buildingData.Health,
                Power = buildingData.Power,
                DataSize = DataSize
            };
        }

        public BuildingMemory CreateDefault(int ownerId, string definition, int? health = null, int? power = null)
        {
            return new BuildingMemory()
            {
                OwnerId = ownerId,
                Definition = definition,
                Health = health ?? BaseHealth,
                Power = power ?? BasePower,
                DataSize = DataSize
            };
        }
    }
}
