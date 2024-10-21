using System.Text;
using Source.Logic.Data;
using Source.Utility;

namespace Source.Logic.Events
{
    public static class EventUtility
    {
        public static bool TryGetUnitAtSlot(this BattlefieldStorage battlefieldStorage, int slot, StringBuilder logBuilder, out BattlefieldItem item, out Unit unit){
            var failedLog = $"Failed to get unit at slot {slot} in {battlefieldStorage}: ";

            if(!TryGetBattlefieldItemAtSlot(battlefieldStorage, slot, logBuilder, out item))
            {
                logBuilder.AppendLine(failedLog + "battlefield item unit is on does not exit (null)");
                unit = null;
                return false;
            }

            if (item.Unit == null)
            {
                logBuilder.AppendLine(failedLog + "unit does not exist (null)");
                unit = null;
                return false;
            }

            unit = item.Unit;
            return true;
        }
        
        public static bool TryGetBuildingAtSlot(this BattlefieldStorage battlefieldStorage, int slot, StringBuilder logBuilder, out BattlefieldItem item, out Building building){
            var failedLog = $"Failed to get building at slot {slot} in {battlefieldStorage}: ";

            if(!TryGetBattlefieldItemAtSlot(battlefieldStorage, slot, logBuilder, out item))
            {
                logBuilder.AppendLine(failedLog + "battlefield item building is on does not exit (null)");
                building = null;
                return false;
            }

            if (item.Building == null)
            {
                logBuilder.AppendLine(failedLog + "building does not exist (null)");
                building = null;
                return false;
            }

            building = item.Building;
            return true;
        }

        
        public static bool TryGetBattlefieldItemAtSlot(this BattlefieldStorage battlefieldStorage, int slot, StringBuilder logBuilder, out BattlefieldItem item)
        {
            var failedLog = $"Failed to get battlefield item at slot {slot} in {battlefieldStorage}: ";
            
            if (!battlefieldStorage.Items.InBounds(slot))
            {
                logBuilder.AppendLine(failedLog + "slot not in bounds");
                item = null;
                return false;
            }

            var itemAtSlot = battlefieldStorage.Items[slot];
            if(itemAtSlot == null){
                logBuilder.AppendLine(failedLog + "battlefield item at slot does not exist");
                item = null;
                return false;
            }

            item = itemAtSlot;
            return true;
        }
    }
}