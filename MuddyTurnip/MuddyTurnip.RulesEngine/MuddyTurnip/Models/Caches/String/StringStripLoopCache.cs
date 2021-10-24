using Microsoft.ApplicationInspector.RulesEngine;
using MuddyTurnip.RulesEngine;
using MuddyTurnip.RulesEngine.Commands;
using System.Collections.Generic;
using System.Text;

namespace MuddyTurnip.Metrics.Engine
{
    public class StringStripLoopCache
    {
        public StringBuilder StringContent { get; }
        public StringSettings StringSettings { get; set; }
        public StringState? Current { get; set; }
        public OutputBoundaries OutputBoundaries { get; }

        public StringStripLoopCache(
            StringBuilder stringContent,
            StringSettings stringSettinge,
            OutputBoundaries outputBoundaries)
        {
            StringContent = stringContent;
            StringSettings = stringSettinge;
            OutputBoundaries = outputBoundaries;
        }
    }
}
