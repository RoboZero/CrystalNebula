using Source.Logic.Data;
using Source.Utility;
using UnityEngine;

namespace Source.Visuals.Battlefield
{
    [CreateAssetMenu(fileName = "UnitName", menuName = "Game/Unit")]
    public class UnitDataSO : DescriptionBaseSO
    {
        public UnitData UnitData => unitData;

        [SerializeField] private UnitData unitData;
    }
}
