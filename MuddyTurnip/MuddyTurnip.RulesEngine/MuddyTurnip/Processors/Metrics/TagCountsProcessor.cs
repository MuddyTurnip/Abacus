using MuddyTurnip.RulesEngine;
using MuddyTurnip.RulesEngine.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MuddyTurnip.Metrics.Engine
{
    public class TagCountsProcessor
    {
        public static void Aggregate(MetricsBlock metrics)
        {
            foreach (MetricsBlock childMetrics in metrics.ChildBlocks)
            {
                Aggregate(childMetrics);
                metrics.TagCounts.MergeTagCounts(childMetrics.TagCounts);
            }
        }
    }
}
