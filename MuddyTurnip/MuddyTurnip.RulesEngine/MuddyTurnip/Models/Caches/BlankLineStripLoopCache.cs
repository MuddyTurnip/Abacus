using Microsoft.ApplicationInspector.RulesEngine;
using MuddyTurnip.RulesEngine.Commands;
using System.Collections.Generic;

namespace MuddyTurnip.Metrics.Engine
{
    public class BlankLineStripLoopCache
    {
        public List<MtBoundary> InputBoundaries { get; }
        public List<MtBoundary> OutputBoundaries { get; }
        public int InputCounter { get; set; }
        public int InputAdjustment { get; set; }
        public int OutputAdjustment { get; set; }

        public BlankLineStripLoopCache(
            List<MtBoundary> commentBoundaries,
            List<MtBoundary> allBoundaries)
        {
            OutputBoundaries = allBoundaries;
            InputBoundaries = commentBoundaries;
        }
    }
}
