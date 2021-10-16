using MuddyTurnip.Metrics.Engine;
using System;
using System.Text.RegularExpressions;

namespace MuddyTurnip.RulesEngine
{
    internal static class CSharpFlagInterpreter
    {
        internal static void CheckAndAddFlag(
            this BlockStats stats,
            Match match,
            string flagName)
        {
            string flag = match.GetGroupValue(flagName);

            if (String.Equals(flag, flagName, StringComparison.Ordinal))
            {
                stats.Flags.Add(flagName);
            }
        }
    }
}
