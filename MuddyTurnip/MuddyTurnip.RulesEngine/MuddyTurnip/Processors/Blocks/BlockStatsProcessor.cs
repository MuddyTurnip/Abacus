using MuddyTurnip.RulesEngine;

namespace MuddyTurnip.Metrics.Engine
{
    public class BlockStatsProcessor
    {
        public static void Aggregate(
            MetricsRecord metricsRecord,
            BlockTextContainer codeContainer)
        {
            if (codeContainer?.BlockStatsCache?.BlockStats is { })
            {
                metricsRecord.Blocks = codeContainer.BlockStatsCache.BlockStats;
            }
        }
    }
}

