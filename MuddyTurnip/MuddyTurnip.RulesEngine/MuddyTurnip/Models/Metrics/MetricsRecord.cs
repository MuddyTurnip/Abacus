using MuddyTurnip.RulesEngine;
using MuddyTurnip.RulesEngine.Commands;
using System.Collections.Generic;

namespace MuddyTurnip.Metrics.Engine
{
    public class MetricsRecord
    {
        public List<TagCounter> TagCounts { get; } = new();
        public List<BlockStats> Blocks { get; set; } = new();
        public List<MtMatchRecord> Matches { get; set; } = new();
        public List<MetricsBlock> Metrics { get; set; } = new();

        public MetricsRecord()
        {
        }
    }
}
