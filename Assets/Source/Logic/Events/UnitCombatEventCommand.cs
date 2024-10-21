using System.Text;
using Source.Logic.Data;
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
            var logBuilder = new StringBuilder();
            logBuilder.AppendLine($"{ID} Unit combat started. Initiator slot: {initiatorSlot}, Responder slot: {responderSlot}");
            
            if (!battlefieldStorage.TryGetUnitAtSlot(initiatorSlot, logBuilder, out _, out var initiatorUnit))
            {
                logBuilder.AppendLine($"Failed to start combat: initiator unit on {initiatorSlot} does not exist. (null)");
                Debug.Log(logBuilder);
                return false;
            }

            if (!battlefieldStorage.TryGetUnitAtSlot(responderSlot, logBuilder, out _, out var responderUnit))
            {
                logBuilder.AppendLine($"Failed to start combat: responder unit on {responderSlot} does not exist (null)");
                Debug.Log(logBuilder);
                return false;
            }

            logBuilder.AppendLine($"Found units on slots. Initiator: {initiatorUnit}, Responder: {responderUnit}");

            Attack(initiatorUnit, responderUnit, logBuilder);
            if (IsUnitDead(responderUnit))
            {
                logBuilder.AppendLine($"Responder has died, cannot counter attack");
                eventTracker.AddEvent(new UnitDeathEventCommand(battlefieldStorage, responderSlot));

                if (tryMoveAfterCombat)
                {
                    // TODO: Let unit decide whether it should move after combat
                    eventTracker.AddEvent(new MoveUnitEventCommand(
                        eventTracker,
                        battlefieldStorage,
                        initiatorSlot,
                        responderSlot,
                        false,
                        false
                        ));
                }
            }
            else
            {
                Attack(responderUnit, initiatorUnit, logBuilder);
            }
            
            if (IsUnitDead(initiatorUnit))
            {
                logBuilder.AppendLine($"Initiator has died");
                eventTracker.AddEvent(new UnitDeathEventCommand(battlefieldStorage, initiatorSlot));
            }

            Debug.Log(logBuilder);
            return true;
        }

        private void Attack(Unit attacker, Unit defender, StringBuilder logBuilder)
        {
            logBuilder.AppendLine($"Attacker {attacker.Definition} has {attacker.Power} power, deals {attacker.Power} damage to defenders's {defender.Health} health");
            defender.Health -= attacker.Power;
            logBuilder.AppendLine($"Defender {defender.Definition} now at {defender.Health} health");
        }

        private bool IsUnitDead(Unit unit)
        {
            return unit.Health <= 0;
        }
    }
}