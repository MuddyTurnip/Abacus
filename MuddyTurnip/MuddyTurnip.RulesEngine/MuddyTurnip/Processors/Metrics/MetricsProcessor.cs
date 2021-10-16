using MuddyTurnip.RulesEngine;
using MuddyTurnip.RulesEngine.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MuddyTurnip.Metrics.Engine
{
    public class MetricsProcessor
    {
        public static void Aggregate(
            MetricsRecord metrics,
            BlockTextContainer codeContainer)
        {
            if (metrics.Matches.Count() == 0)
            {
                return;
            }

            string fileName = metrics.Matches.First()?.FileName ?? String.Empty;

            if (String.IsNullOrWhiteSpace(fileName))
            {
                return;
            }

            MatchProcessor.Aggregate(metrics);

            BlockStatsProcessor.Aggregate(
                metrics,
                codeContainer
            );
        }
    }
}
