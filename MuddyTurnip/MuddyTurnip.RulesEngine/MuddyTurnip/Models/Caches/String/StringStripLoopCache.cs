using Microsoft.ApplicationInspector.RulesEngine;
using MuddyTurnip.RulesEngine;
using MuddyTurnip.RulesEngine.Commands;
using System.Collections.Generic;
using System.Text;

namespace MuddyTurnip.Metrics.Engine
{
    public class StringStripLoopCache
    {
        public List<MtBoundary> InputBoundaries { get; }
        public List<MtBoundary> OutputBoundaries { get; }
        public StringBuilder StringContent { get; }
        public int InputCounter { get; set; }
        public StringSettings StringSettings { get; set; }
        public int InputAdjustment { get; set; }
        public int OutputAdjustment { get; set; }
        public StringState? Current { get; set; }

        public StringStripLoopCache(
            List<MtBoundary> inputBoundaries,
            List<MtBoundary> outputBoundaries,
            StringBuilder stringContent,
            StringSettings stringSettinge)
        {
            OutputBoundaries = outputBoundaries;
            InputBoundaries = inputBoundaries;
            StringContent = stringContent;
            StringSettings = stringSettinge;
        }
    }
}
