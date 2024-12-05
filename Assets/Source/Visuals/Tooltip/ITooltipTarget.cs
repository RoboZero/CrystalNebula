using System.Collections.Generic;

namespace Source.Visuals.Tooltip
{
    public interface ITooltipTarget
    {
        public HashSet<TooltipContent> GetContent();
    }
}
