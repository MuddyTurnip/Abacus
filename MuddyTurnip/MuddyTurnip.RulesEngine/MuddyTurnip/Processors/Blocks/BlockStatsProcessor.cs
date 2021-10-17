using MuddyTurnip.RulesEngine;
using System.Collections.Generic;

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

            List<MetricsBlock> metricsList = new();

            BuildMetricBlocks(
                codeContainer?.BlockStatsCache?.RootBlockStats,
                metricsRecord.Metrics
            );
        }

        private static void BuildMetricBlocks(
            BlockStats? parentBlock,
            List<MetricsBlock> metricsList)
        {
            if (parentBlock is null)
            {
                return;
            }

            foreach (BlockStats child in parentBlock.ChildBlocks)
            {
                if (child.PrintMetrics)
                {
                    MetricsBlock childMetrics = new()
                    {
                        Type = child.Type,
                        Signature = child.CleanedSignature,
                        Content = child.Content,
                        Partial = child.Flags.Contains("Partial"),
                        Depth = child.Depth,
                        SyblingCount = child.SyblingCount,
                        AdjustedOpenIndex = child.AdjustedOpenIndex,
                        AdjustedCloseIndex = child.AdjustedCloseIndex,
                        BlockStartLocation = child.BlockStartLocation,
                        BlockEndLocation = child.BlockEndLocation
                    };

                    metricsList.Add(childMetrics);

                    BuildMetricBlocks(
                        child,
                        childMetrics.ChildBlocks
                    );
                }
                else
                {
                    BuildMetricBlocks(
                        child,
                        metricsList
                    );
                }
            }
        }
    }
}

