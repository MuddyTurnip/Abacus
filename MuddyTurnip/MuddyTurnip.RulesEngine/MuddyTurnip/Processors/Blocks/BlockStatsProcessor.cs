using MuddyTurnip.RulesEngine;

namespace MuddyTurnip.Metrics.Engine
{
    public class BlockStatsProcessor
    {
        public static void Aggregate(
            AbacusRecord abacusRecord,
            BlockTextContainer codeContainer)
        {
            if (codeContainer?.BlockStatsCache?.BlockStats is { })
            {
                abacusRecord.Blocks = codeContainer.BlockStatsCache.BlockStats;
            }
        }
    }
}

