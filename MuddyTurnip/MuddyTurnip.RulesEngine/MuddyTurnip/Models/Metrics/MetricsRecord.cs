using MuddyTurnip.RulesEngine;
using MuddyTurnip.RulesEngine.Commands;
using System.Collections.Generic;

namespace MuddyTurnip.Metrics.Engine
{
    public class AbacusRecord
    {
        public string FilePath { get; set; }
        public List<TagCounter> TagCounts { get; } = new ();
        public List<BlockStats> Blocks { get; set; } = new();
        public List<MtMatchRecord> Matches { get; set; } = new();
        public MetricsRecord Metrics { get; set; }

        public AbacusRecord(string filePath)
        {
            FilePath = filePath;
            Metrics = new();
        }
    }
}
