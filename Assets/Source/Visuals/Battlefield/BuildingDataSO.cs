using Source.Logic.Data;
using Source.Utility;
using UnityEngine;

namespace Source.Visuals.Battlefield
{
    [CreateAssetMenu(fileName = "BuildingName", menuName = "Game/Building")]
    public class BuildingDataSO : DescriptionBaseSO
    {
        public BuildingData BuildingData => buildingData;

        [SerializeField] private BuildingData buildingData;
    }
}
