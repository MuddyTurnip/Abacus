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
                        OpenIndex = child.AdjustedMatchStart < child.AdjustedOpenIndex ? child.AdjustedMatchStart : child.AdjustedOpenIndex,
                        CloseIndex = child.AdjustedCloseIndex,
                        BlockStartLocation = child.BlockStartLocation,
                        BlockEndLocation = child.BlockEndLocation,
                        Block = child
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

