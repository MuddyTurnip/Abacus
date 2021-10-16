using MuddyTurnip.Metrics.Engine;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MuddyTurnip.RulesEngine
{
    internal static class CSharpComponentInterpreter
    {
        internal static void FindComponent(
            this List<BlockStats> blockStats,
            ComponentSettings componentSettings,
            string text,
            IBoundaryCounter boundaryCounter,
            StringBuilder cleanedBuilder)
        {
            int matchEnd;
            Regex regex;
            MatchCollection matches;

            // Parents should be run first then children
            // More specific searches should be run before less
            // ie Constructors before methods etc
            // stats named will be skipped on following matches

            foreach (string pattern in componentSettings.Patterns)
            {
                regex = new Regex(pattern);
                matches = regex.Matches(text);

                foreach (Match match in matches)
                {
                    matchEnd = match.Index + match.Value.Length;

                    foreach (BlockStats stats in blockStats)
                    {
                        if (stats.Settings.BlockType != componentSettings.BlockType
                            || stats.Settings.Model != "Block"
                            || !String.IsNullOrWhiteSpace(stats.Name))
                        {
                            // block already named so can't do it again
                            continue;
                        }

                        if (stats.OpenIndex == matchEnd)
                        {
                            stats.MatchStart = match.Index;
                            stats.MatchEnd = match.Index + match.Value.Length;
                            stats.AdjustedMatchStart = boundaryCounter.AdjustLineNumber(stats.MatchStart);
                            stats.AdjustedMatchEnd = boundaryCounter.AdjustLineNumber(stats.MatchEnd);
                            stats.MatchStartLocation = boundaryCounter.GetLocation(stats.AdjustedMatchStart);
                            stats.MatchEndLocation = boundaryCounter.GetLocation(stats.AdjustedMatchEnd);

                            stats.Name = match.Groups[componentSettings.Name].Value;
                            stats.Name = stats.Name.CleanSpaces();
                            stats.Type = componentSettings.Name;
                            stats.Signature = match.GetGroupValue("signature");

                            stats.CheckAndAddFlag(
                                match,
                                "partial");

                            stats.CleanedSignature = match.CleanSignature(
                                cleanedBuilder,
                                componentSettings.Name,
                                stats.Signature
                            );

                            stats.Value = match.Value;
                            stats.QualifiesName = componentSettings.QualifiesName;

                            stats.FindUnits(
                                componentSettings,
                                text,
                                boundaryCounter);

                            break;
                        }
                        else if (stats.OpenIndex > matchEnd)
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}
