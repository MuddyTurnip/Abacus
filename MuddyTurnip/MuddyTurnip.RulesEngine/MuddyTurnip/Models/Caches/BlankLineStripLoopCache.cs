using Microsoft.ApplicationInspector.RulesEngine;
using System.Collections.Generic;

namespace MuddyTurnip.Metrics.Engine
{
    public class BlankLineStripLoopCache
    {
        public List<Boundary> InputBoundaries { get; }
        public List<Boundary> OutputBoundaries { get; }
        public int InputCounter { get; set; }
        public int InputAdjustment { get; set; }
        public int OutputAdjustment { get; set; }

        public BlankLineStripLoopCache(
            List<Boundary> commentBoundaries,
            List<Boundary> allBoundaries)
        {
            OutputBoundaries = allBoundaries;
            InputBoundaries = commentBoundaries;
        }
    }
}
