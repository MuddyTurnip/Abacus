using MuddyTurnip.RulesEngine.Commands;
using System.Collections.Generic;

namespace MuddyTurnip.Metrics.Engine
{
    public class MetricsRecord
    {
        public string FilePath { get; set; }
        public List<TagCounter> TagCounts { get; } = new ();
        public List<MtMatchRecord> Matches { get; set; } = new ();
        public List<ClassMetricsRecord> ClassMetrics { get; set; } = new ();
        public List<MethodMetricsRecord> MethodMetrics { get; set; } = new();

        public MetricsRecord(string filePath)
        {
            FilePath = filePath;
        }
    }
}
