using MuddyTurnip.RulesEngine;
using MuddyTurnip.RulesEngine.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MuddyTurnip.Metrics.Engine
{
    public class ErrorProcessor
    {
        public static void Aggregate(MetricsBlock metrics)
        {
            foreach (BlockStatsError error in metrics.Errors)
            {
                metrics.TagCounts.IncrementTagCount($"Abacus.Error.{error.Type}");
            }
        }
    }
}
