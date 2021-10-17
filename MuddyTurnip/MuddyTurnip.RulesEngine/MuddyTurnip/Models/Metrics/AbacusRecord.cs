using MuddyTurnip.RulesEngine.Commands;
using System.Collections.Generic;

namespace MuddyTurnip.Metrics.Engine
{
    public class MetricsRecord
    {
        public List<ClassMetricsRecord> ClassMetrics { get; set; } = new ();
        public List<MethodMetricsRecord> MethodMetrics { get; set; } = new();

        public MetricsRecord()
        {
        }
    }
}
