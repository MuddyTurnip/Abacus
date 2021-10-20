using MuddyTurnip.RulesEngine;
using MuddyTurnip.RulesEngine.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MuddyTurnip.Metrics.Engine
{
    public class LineCountsProcessor
    {
        public static void Aggregate(
            MetricsRecord metricsRecord,
            BlockTextContainer codeContainer)
        {
            DistributeLineCounts(
                metricsRecord,
                codeContainer
            );

            ConvertLineCountsToTagCounts(metricsRecord.Structure);
            AggregateChildren(metricsRecord.Structure);
        }

        private static void ConvertLineCountsToTagCounts(MetricsBlock metrics)
        {
            List<TagCounter> tagCounts = metrics.TagCounts;
            LineCounts counts;

            for (int i = 0; i < metrics.LineCounts.Count; i++)
            {
                counts = metrics.LineCounts[i];

                tagCounts.PrintTagCount(
                    "Spaces",
                    counts.SpacesCount,
                    i
                );

                tagCounts.PrintTagCount(
                    "Uppercase",
                    counts.UpperCaseCount,
                    counts.Number
                );

                tagCounts.PrintTagCount(
                    "Lowercase",
                    counts.LowerCaseCount,
                    counts.Number
                );

                tagCounts.PrintTagCount(
                    "_",
                    counts.UnderscoreCount,
                    counts.Number
                );

                tagCounts.PrintTagCount(
                    "-",
                    counts.HyphenCount,
                    counts.Number
                );

                tagCounts.PrintTagCount(
                    "Length",
                    counts.Length,
                    counts.Number
                );

                tagCounts.PrintTagCount(
                    "Words",
                    counts.WordCount,
                    counts.Number
                );
            }
        }

        private static void AggregateChildren(MetricsBlock metrics)
        {
            foreach (MetricsBlock child in metrics.ChildBlocks)
            {
                ConvertLineCountsToTagCounts(child);
                AggregateChildren(child);
            }
        }

        private static void DistributeLineCounts(
            MetricsRecord metricsRecord,
            BlockTextContainer codeContainer)
        {
            if (codeContainer?.BlockStatsCache?.BlockStats is null)
            {
                return;
            }

            List<LineCounts> lineCounts = codeContainer.BlockStatsCache.LineCountList;
            lineCounts.Sort(LineCountsExtensions.Compare);
            LineCounts counts;
            bool success;

            for (int i = 0; i < lineCounts.Count; i++)
            {
                counts = lineCounts[i];
                counts.Number = i + 1;

                success = MatchBlock(
                     counts,
                     metricsRecord.Structure
                 );

                if (!success)
                {
                    metricsRecord.Structure.LineCounts.Add(counts);
                }
            }
        }

        private static bool MatchBlock(
            LineCounts counts,
            MetricsBlock metrics)
        {
            MetricsBlock block;
            bool success;

            for (int i = 0; i < metrics.ChildBlocks.Count; i++)
            {
                block = metrics.ChildBlocks[i];

                if (block.LineStartIndex <= counts.StartIndex
                    && block.CloseIndex >= counts.StartIndex)
                {
                    success = MatchBlock(
                        counts,
                        block
                    );

                    if (!success)
                    {
                        block.LineCounts.Add(counts);
                    }

                    return true;
                }
            }

            return false;
        }

    }
}
