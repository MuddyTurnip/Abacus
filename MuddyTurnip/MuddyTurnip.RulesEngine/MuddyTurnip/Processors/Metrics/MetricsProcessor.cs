﻿using MuddyTurnip.RulesEngine;
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

            BlockStatsProcessor.Aggregate(
                metricsRecord,
                codeContainer
            );

            MatchProcessor.Aggregate(metricsRecord.Structure);
            ErrorProcessor.Aggregate(metricsRecord.Structure);
            TagCountsProcessor.Aggregate(metricsRecord.Structure);
        }
    }
}
