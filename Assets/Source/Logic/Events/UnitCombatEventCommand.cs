using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Logic.State;
using Source.Logic.State.Battlefield;
using Source.Logic.State.LineItems;
using Source.Logic.State.LineItems.Units;
using Source.Utility;
using UnityEngine;

namespace Source.Logic.Events
{
    public class UnitCombatEventCommand : EventCommand
    {
        private EventTracker eventTracker;
        private LineStorage<BattlefieldItem> battlefieldStorage;
        private int initiatorSlot;
        private int responderSlot;
        private bool tryMoveAfterCombat;

        public UnitCombatEventCommand(
            EventTracker eventTracker,
            LineStorage<BattlefieldItem> battlefieldStorage,
            int initiatorSlot,
            int responderSlot,
            bool tryMoveAfterCombat
        ) : base(eventTracker)
        {
            this.eventTracker = eventTracker;
            this.battlefieldStorage = battlefieldStorage;
            this.initiatorSlot = initiatorSlot;
            this.responderSlot = responderSlot;
            this.tryMoveAfterCombat = tryMoveAfterCombat;
        }
        
        public override async UniTask Apply(CancellationToken cancellationToken)
        {
            status = EventStatus.Started;
            AddLog($"Unit combat started. Initiator slot: {initiatorSlot}, Responder slot: {responderSlot}");
            
            if (!TryGetUnitAtSlot(battlefieldStorage, initiatorSlot, out _, out var initiatorUnit))
            {
                AddLog($"Failed to start combat: initiator unit on {initiatorSlot} does not exist. (null)");
                status = EventStatus.Failed;
                return;
            }

            if (!initiatorUnit.CanEngageCombat)
            {
                AddLog($"Failed to start combat: initiator unit on {initiatorSlot} cannot initiate combat. ");
                status = EventStatus.Failed;
                return;
            }

            if (!TryGetUnitAtSlot(battlefieldStorage, responderSlot, out _, out var responderUnit))
            {
                AddLog($"Failed to start combat: responder unit on {responderSlot} does not exist (null)");
                status = EventStatus.Failed;
                return;
            }

            AddLog($"Found units on slots. Initiator: {initiatorUnit}, Responder: {responderUnit}");

            Attack(initiatorUnit, responderUnit);
            if (IsUnitDead(responderUnit))
            {
                AddLog($"Responder has died, cannot counter attack");

                await ApplyChildEventWithLog(new UnitDeathEventCommand(eventTracker, battlefieldStorage, responderSlot), cancellationToken);

                if (tryMoveAfterCombat)
                {
                    AddLog("Moving to responders slot after their death.");
                    // TODO: Let unit decide whether it should move after combat
                    await ApplyChildEventWithLog(new TeleportUnitEventCommand(
                        eventTracker,
                        battlefieldStorage,
                        initiatorSlot,
                        responderSlot,
                        null
                    ), cancellationToken);
                }
            }
            else
            {
                Attack(responderUnit, initiatorUnit);
            }
            
            if (IsUnitDead(initiatorUnit))
            {
                AddLog($"Initiator has died");
                await ApplyChildEventWithLog(new UnitDeathEventCommand(eventTracker, battlefieldStorage, initiatorSlot), cancellationToken);
            }

            status = EventStatus.Success;
        }

        private void Attack(UnitMemory attacker, UnitMemory defender)
        {
            AddLog($"Attacker {attacker.Definition} has {attacker.Power} power, deals {attacker.Power} damage to defenders's {defender.Health} health");
            defender.Health -= attacker.Power;
            AddLog($"Defender {defender.Definition} now at {defender.Health} health");
        }

        private bool IsUnitDead(UnitMemory unit)
        {
            return unit.Health <= 0;
        }
    }
}