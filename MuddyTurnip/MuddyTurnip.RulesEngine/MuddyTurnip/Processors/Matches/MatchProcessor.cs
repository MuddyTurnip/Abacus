using MuddyTurnip.RulesEngine.Commands;
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
    }
}
