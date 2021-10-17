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
            AbacusRecord abacusRecord,
            BlockTextContainer codeContainer)
        {
            if (abacusRecord.Matches.Count() == 0)
            {
                return;
            }

            string fileName = abacusRecord.Matches.First()?.FileName ?? String.Empty;

            if (String.IsNullOrWhiteSpace(fileName))
            {
                return;
            }

            MatchProcessor.Aggregate(abacusRecord);

            BlockStatsProcessor.Aggregate(
                abacusRecord,
                codeContainer
            );
        }
    }
}
