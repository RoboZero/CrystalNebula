using System.Collections.Generic;

namespace Source.Visuals.Tooltip
{
    public interface ITooltipTarget
    {
        public void UpdateContent(TooltipVisual tooltipVisual);
    }
}
