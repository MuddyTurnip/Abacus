using MuddyTurnip.RulesEngine;
using MuddyTurnip.RulesEngine.Commands;
using Newtonsoft.Json;
using System;
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

            BuildMetricBlocks(
                codeContainer?.BlockStatsCache?.RootBlockStats,
                metricsRecord.Structure
            );

            DistributeMatches(metricsRecord);
            MatchProcessor.Aggregate(metricsRecord.Structure);
        }

        private static void DistributeMatches(MetricsRecord metricsRecord)
        {
            metricsRecord.Matches.Sort(MtMatchRecordExtensions.Compare);
            bool success;

            foreach (MtMatchRecord match in metricsRecord.Matches)
            {
                success = MatchBlock(
                    match,
                    metricsRecord.Structure
                );

                if (!success)
                {
                    metricsRecord.Structure.Matches.Add(match);
                }
            }
        }

        private static bool MatchBlock(
            MtMatchRecord match,
            MetricsBlock metrics)
        {
            MetricsBlock block;
            bool success;

            for (int i = 0; i < metrics.ChildBlocks.Count; i++)
            {
                block = metrics.ChildBlocks[i];

                if (block.OpenIndex <= match.StartIndex
                    && block.CloseIndex >= match.EndIndex)
                {
                    success = MatchBlock(
                        match,
                        block
                    );

                    if (!success)
                    {
                        block.Matches.Add(match);
                    }

                    return true;
                }
            }

            return false;
        }

        private static void BuildMetricBlocks(
            BlockStats? parentBlock,
            MetricsBlock metrics)
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
                        Partial = child.Flags.Contains("partial"),
                        Depth = child.Depth,
                        SyblingCount = child.SyblingCount,
                        OpenIndex = child.AdjustedOpenIndex,
                        CloseIndex = child.AdjustedCloseIndex,
                        BlockStartLocation = child.BlockStartLocation,
                        BlockEndLocation = child.BlockEndLocation
                    };

                    metrics.ChildBlocks.Add(childMetrics);

                    BuildMetricBlocks(
                        child,
                        childMetrics
                    );
                }
                else
                {
                    BuildMetricBlocks(
                        child,
                        metrics
                    );
                }
            }
        }
    }
}

