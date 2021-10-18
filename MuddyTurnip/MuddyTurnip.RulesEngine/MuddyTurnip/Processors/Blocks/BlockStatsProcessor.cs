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

        private static List<BlockStatsError> BuildMetricBlocks(
            BlockStats? parentBlock,
            MetricsBlock metrics)
        {
            List<BlockStatsError> errors = new();

            if (parentBlock is null)
            {
                return errors;
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

                    childMetrics.Errors = BuildMetricBlocks(
                        child,
                        childMetrics
                    );
                }
                else
                {
                    List<BlockStatsError> childErrors = BuildMetricBlocks(
                        child,
                        metrics
                    );

                    errors.AddRange(childErrors);
                }
            }

            return errors;
        }
    }
}

