using MuddyTurnip.RulesEngine;
using MuddyTurnip.RulesEngine.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MuddyTurnip.Metrics.Engine
{
    public class MetricsProcessor
    {
        public static void Aggregate(
            MetricsRecord metricsRecord,
            BlockTextContainer codeContainer)
        {
            if (metricsRecord.Matches.Count() == 0)
            {
                return;
            }

            string fileName = metricsRecord.Matches.First()?.FileName ?? String.Empty;

            if (String.IsNullOrWhiteSpace(fileName))
            {
                return;
            }

            PreTagCountAggregator(
                metricsRecord,
                codeContainer
            );

            TagCountsProcessor.Aggregate(metricsRecord.Structure);

            PostTagCountAggregator(
                metricsRecord,
                codeContainer);
        }

        private static void PreTagCountAggregator(
            MetricsRecord metricsRecord,
            BlockTextContainer codeContainer)
        {
            BlockStatsProcessor.Aggregate(
                metricsRecord,
                codeContainer
            );

            UnitsOfWorkProcessor.Aggregate(
                metricsRecord,
                codeContainer
            );

            LineCountsProcessor.Aggregate(
                metricsRecord,
                codeContainer
            );

            MatchProcessor.DistributeMatches(metricsRecord);

            MatchProcessor.Aggregate(metricsRecord);
            ErrorProcessor.Aggregate(metricsRecord);
            BlockStatsProcessor.AggregateBlockDepth(metricsRecord);

        }

        private static void PostTagCountAggregator(
            MetricsRecord metricsRecord,
            BlockTextContainer codeContainer)
        {
            LocalBlockStatsProcessor.Aggregate(metricsRecord);
        }
    }
}
