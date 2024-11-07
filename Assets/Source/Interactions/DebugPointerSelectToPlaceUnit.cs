using System.Linq;
using Source.Input;
using Source.Logic;
using Source.Logic.Events;
using Source.Logic.State;
using Source.Serialization;
using Source.Serialization.Data;
using Source.Serialization.Samples;
using Source.Visuals.BattlefieldStorage;
using UnityEngine;

namespace Source.Interactions
{
    public class DebugPointerSelectToPlaceUnit : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private PlayerInteractions playerInteractions;
        [SerializeField] private BattlefieldStorageBehavior battlefieldStorageBehavior;
        [SerializeField] private BattlefieldStorageVisual battlefieldStorageVisual;
        [SerializeField] private EventTracker eventTracker;
        [SerializeField] private InputReaderSO inputReader;
        [SerializeField] private UnitMemoryDataSO unitMemoryDataSO;
        [SerializeField] private BuildingMemoryDataSO buildingMemoryDataSO;
        
        private void OnEnable()
        {
            inputReader.HoldPressedEvent += OnHoldPressed;
            
            var s = new JsonDataService();
            s.SaveData("/GameState.json", SampleData.TestState1, false);
        }

        private void OnDisable()
        {
            inputReader.HoldPressedEvent -= OnHoldPressed;
            //inputReader.CommandPressedEvent += OnCommandPress;
        }
        
        private void OnHoldPressed()
        {
            MoveUnit();
        }

        private void PlaceItemOnBattlefield()
        {
            var interactedSlots = playerInteractions.Interacted
                .OfType<BattlefieldItemVisual>()
                .Select(visual => visual.TrackedSlot)
                .ToList();
            
            if (interactedSlots.Count <= 0) return;
            
            if (unitMemoryDataSO != null)
            {
                eventTracker.AddEvent(new CreateUnitsEventCommand(
                    battlefieldStorageBehavior.State, 
                    interactedSlots,
                    unitMemoryDataSO.CreateDefault(0, "Units/Guardian"),
                    false
                ));
            }
            if (buildingMemoryDataSO != null)
            {
                eventTracker.AddEvent(new CreateBuildingsEventCommand(
                    battlefieldStorageBehavior.State, 
                    interactedSlots,
                    buildingMemoryDataSO.CreateDefault(0, "Buildings/Flag"),
                    false
                ));
            }
        }

        private void MoveUnit()
        {
            var interactedSlots = playerInteractions.Interacted
                .OfType<BattlefieldItemVisual>()
                .Select(visual => visual.TrackedSlot)
                .ToList();
            var hoveredSlots = playerInteractions.Hovered
                .OfType<BattlefieldItemVisual>()
                .Select(visual => visual.TrackedSlot)
                .ToList();

            if (interactedSlots.Count <= 0 || hoveredSlots.Count <= 0) return;
            
            eventTracker.AddEvent(new TeleportUnitEventCommand(
                eventTracker,
                battlefieldStorageBehavior.State,
                interactedSlots[0],
                hoveredSlots[0],
                null
            ));
        }
    }
}