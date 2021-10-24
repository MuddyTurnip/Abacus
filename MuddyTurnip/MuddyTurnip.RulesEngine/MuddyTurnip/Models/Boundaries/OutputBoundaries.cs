using MuddyTurnip.RulesEngine.Commands;
using System.Collections.Generic;
using System.Diagnostics;

namespace MuddyTurnip.RulesEngine
{
    [DebuggerDisplay("{Type}")]
    public class OutputBoundaries
    {
        public int Adjustment { get; set; }
        public List<MtBoundary> Boundaries { get; }

        public OutputBoundaries(List<MtBoundary> outputBoundaries)
        {
            Boundaries = outputBoundaries;
        }
    }
}