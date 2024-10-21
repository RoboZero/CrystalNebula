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

        public UnitCombatEventCommand(
            EventTracker eventTracker,
            BattlefieldStorage battlefieldStorage,
            int initiatorSlot,
            int responderSlot
        )
        {
            this.eventTracker = eventTracker;
            this.battlefieldStorage = battlefieldStorage;
            this.initiatorSlot = initiatorSlot;
            this.responderSlot = responderSlot;
        }
        
        public override bool Perform()
        {
            var logBuilder = new StringBuilder();
            logBuilder.AppendLine($"{ID} Unit combat started. Initiator slot: {initiatorSlot}, Responder slot: {responderSlot}");

            if (!battlefieldStorage.TryGetUnitAtSlot(initiatorSlot, logBuilder, out _, out var initiatorUnit)
                || battlefieldStorage.TryGetUnitAtSlot(responderSlot, logBuilder, out _, out var responderUnit))
            {
                Debug.Log(logBuilder);
                return false;
            }

            logBuilder.AppendLine($"{ID} Unit combat started. Initiator: {initiatorUnit.Definition}, Responder: {responderUnit.Definition}");

            Attack(initiatorUnit, responderUnit, logBuilder);
            if (IsUnitDead(responderUnit))
            {
                logBuilder.AppendLine($"Responder has died, cannot counter attack");
                eventTracker.AddEvent(new UnitDeathEventCommand(battlefieldStorage, initiatorSlot));
            }
            else
            {
                Attack(responderUnit, initiatorUnit, logBuilder);
                if (IsUnitDead(initiatorUnit))
                {
                    logBuilder.AppendLine($"Initiator has died");
                    eventTracker.AddEvent(new UnitDeathEventCommand(battlefieldStorage, initiatorSlot));
                }
            }

            Debug.Log(logBuilder);
            return true;
        }

        private void Attack(Unit attacker, Unit defender, StringBuilder logBuilder)
        {
            logBuilder.AppendLine($"Attacker {attacker.Definition} has {attacker.Power}, deals {attacker.Power} damage to defenders's {defender.Health} health");
            defender.Health -= attacker.Power;
            logBuilder.AppendLine($"Defender {defender.Definition} now at {defender.Health} health");
        }

        private bool IsUnitDead(Unit unit)
        {
            return unit.Health <= 0;
        }
    }
}