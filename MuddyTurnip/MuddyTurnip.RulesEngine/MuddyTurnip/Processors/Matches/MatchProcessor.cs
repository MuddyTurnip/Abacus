﻿using MuddyTurnip.RulesEngine.Commands;
using System;
using System.Collections.Generic;

namespace MuddyTurnip.Metrics.Engine
{
    public class MatchProcessor
    {
        public static void Aggregate(MetricsBlock metrics)
        {
            string matchRecordTag;

            foreach (MtMatchRecord matchRecord in metrics.Matches)
            {
                if (matchRecord == null
                    || matchRecord.Tags == null)
                {
                    continue;
                }

                for (int j = 0; j < matchRecord.Tags.Length; j++)
                {
                    matchRecordTag = matchRecord.Tags[j];
                    metrics.TagCounts.IncrementTagCount(matchRecordTag);
                }
            }
        }

        public static void DistributeMatches(MetricsRecord metricsRecord)
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

    }
}
