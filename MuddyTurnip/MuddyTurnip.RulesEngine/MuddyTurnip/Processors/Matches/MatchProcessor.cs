using MuddyTurnip.RulesEngine.Commands;
using System;
using System.Collections.Generic;

namespace MuddyTurnip.Metrics.Engine
{
    public class MatchProcessor
    {
        public static List<TagCounter> Aggregate(MetricsBlock metrics)
        {
            List<TagCounter> tagCounts;

            foreach (MetricsBlock childMetrics in metrics.ChildBlocks)
            {
                tagCounts = Aggregate(childMetrics);
                metrics.TagCounts.MergeTagCounts(tagCounts);
            }

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

            return metrics.TagCounts;
        }
    }
}
