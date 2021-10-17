using MuddyTurnip.RulesEngine.Commands;
using System;
using System.Collections.Generic;

namespace MuddyTurnip.Metrics.Engine
{
    public class MatchProcessor
    {
        public static void Aggregate(MetricsRecord metricsRecord)
        {
            List<TagCounter> fileTagCounts = metricsRecord.TagCounts;
            string matchRecordTag;

            foreach (MtMatchRecord matchRecord in metricsRecord.Matches)
            {
                if (matchRecord == null
                    || matchRecord.Tags == null)
                {
                    continue;
                }

                for (int j = 0; j < matchRecord.Tags.Length; j++)
                {
                    matchRecordTag = matchRecord.Tags[j];

                    IncrementTagCount(
                        fileTagCounts, 
                        matchRecordTag
                    );
                }
            }
        }

        private static void IncrementTagCount(
            List<TagCounter> tagCounts,
            string tag)
        {
            TagCounter tagCount;

            for (int i = 0; i < tagCounts.Count; i++)
            {
                tagCount = tagCounts[i];

                if (String.Equals(tagCount.Tag, tag))
                {
                    tagCount.Count++;

                    return;
                }
            }

            tagCounts.Add(new TagCounter(tag, 1));
        }
    }
}
