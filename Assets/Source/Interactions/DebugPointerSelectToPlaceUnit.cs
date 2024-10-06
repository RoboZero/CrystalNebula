using Source.Input;
using Source.Logic;
using Source.Logic.Data;
using Source.Logic.Events;
using Source.Logic.State;
using Source.Serialization;
using Source.Visuals.Battlefield;
using UnityEngine;

namespace Source.Interactions
{
    public class DebugPointerSelectToPlaceUnit : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private PlayerInteractions playerInteractions;
        [SerializeField] private BattlefieldStorageVisual battlefieldStorageVisual;
        [SerializeField] private EventTracker eventTracker;
        [SerializeField] private InputReaderSO inputReader;
        [SerializeField] private UnitDataSO unitDataSO;
        [SerializeField] private BuildingDataSO buildingDataSO;
        
        private void OnEnable()
        {
            inputReader.HoldPressedEvent += OnHoldPressed;
            
            var s = new JsonDataService();
            s.SaveData("/GameState.json", SampleStates.TestState1, false);
        }

        private void OnDisable()
        {
            inputReader.HoldPressedEvent -= OnHoldPressed;
        }
        
        private void OnHoldPressed()
        {
            if (unitDataSO != null)
            {
                eventTracker.AddEvent(new CreateUnitsEventCommand(
                    battlefieldStorageVisual.ItemStorage, 
                    battlefieldStorageVisual.InteractedVisualIndices,
                    unitDataSO.CreateDefault(0, "Units:Guardian")
                ));
            }
            if (buildingDataSO != null)
            {
                eventTracker.AddEvent(new CreateBuildingsEventCommand(
                    battlefieldStorageVisual.ItemStorage, 
                    battlefieldStorageVisual.InteractedVisualIndices,
                    buildingDataSO.CreateDefault(0, "Buildings:Flag")
                ));
            }
        }

    }
}