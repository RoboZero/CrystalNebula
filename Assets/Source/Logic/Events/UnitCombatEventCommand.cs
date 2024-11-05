using System.Text;
using Source.Logic.State;
using Source.Utility;
using UnityEngine;

namespace Source.Logic.Events
{
    public class UnitCombatEventCommand : EventCommand
    {
        private EventTracker eventTracker;
        private BattlefieldStorage battlefieldStorage;
        private int initiatorSlot;
        private int responderSlot;
        private bool tryMoveAfterCombat;

        public UnitCombatEventCommand(
            EventTracker eventTracker,
            BattlefieldStorage battlefieldStorage,
            int initiatorSlot,
            int responderSlot,
            bool tryMoveAfterCombat
        )
        {
            this.eventTracker = eventTracker;
            this.battlefieldStorage = battlefieldStorage;
            this.initiatorSlot = initiatorSlot;
            this.responderSlot = responderSlot;
            this.tryMoveAfterCombat = tryMoveAfterCombat;
        }
        
        public override bool Perform()
        {
            AddLog($"Unit combat started. Initiator slot: {initiatorSlot}, Responder slot: {responderSlot}");
            
            if (!TryGetUnitAtSlot(battlefieldStorage, initiatorSlot, out _, out var initiatorUnit))
            {
                AddLog($"Failed to start combat: initiator unit on {initiatorSlot} does not exist. (null)");
                return false;
            }

            if (!initiatorUnit.CanEngageCombat)
            {
                AddLog($"Failed to start combat: initiator unit on {initiatorSlot} cannot initiate combat. ");
                return false;
            }

            if (!TryGetUnitAtSlot(battlefieldStorage, responderSlot, out _, out var responderUnit))
            {
                AddLog($"Failed to start combat: responder unit on {responderSlot} does not exist (null)");
                return false;
            }

            AddLog($"Found units on slots. Initiator: {initiatorUnit}, Responder: {responderUnit}");

            Attack(initiatorUnit, responderUnit);
            if (IsUnitDead(responderUnit))
            {
                AddLog($"Responder has died, cannot counter attack");

                PerformChildEventWithLog(new UnitDeathEventCommand(battlefieldStorage, responderSlot));

                if (tryMoveAfterCombat)
                {
                    // TODO: Let unit decide whether it should move after combat
                    PerformChildEventWithLog(new TeleportUnitEventCommand(
                        eventTracker,
                        battlefieldStorage,
                        initiatorSlot,
                        responderSlot,
                        null
                    ));
                }
            }
            else
            {
                Attack(responderUnit, initiatorUnit);
            }
            
            if (IsUnitDead(initiatorUnit))
            {
                AddLog($"Initiator has died");
                PerformChildEventWithLog(new UnitDeathEventCommand(battlefieldStorage, initiatorSlot));
            }

            return true;
        }

        private void Attack(Unit attacker, Unit defender)
        {
            AddLog($"Attacker {attacker.Definition} has {attacker.Power} power, deals {attacker.Power} damage to defenders's {defender.Health} health");
            defender.Health -= attacker.Power;
            AddLog($"Defender {defender.Definition} now at {defender.Health} health");
        }

        private bool IsUnitDead(Unit unit)
        {
            return unit.Health <= 0;
        }
    }
}