using MuddyTurnip.Metrics.Engine;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MuddyTurnip.RulesEngine
{
    internal static class CSharpGroupUnitsInterpreter
    {
        public static void ProcessUnits(
            this BlockStats group,
            BlockStatsCache blockStatsCache,
            GroupSettings groupSettings,
            IBoundaryCounter boundaryCounter,
            string text)
        {
            if (groupSettings.Units.Count == 0)
            {
                return;
            }

            BlockStats? parent = null;
            BlockStats block;

            // Assume last block is Code and this holds the units
            // Find the startlocattion and endlocation then find the block that matches these
            // Then use these are the parent block etc...
            for (int m = group.ChildBlocks.Count - 1; m >= 0; m--)
            {
                block = group.ChildBlocks[m];

                if (block.Settings.BlockType == "Code")
                {
                    parent = block;

                    break;
                }
            }

            if (parent is null)
            {
                throw new NotImplementedException(nameof(parent));
            }

            int matchEnd;
            int matchStart;
            Regex regex;
            MatchCollection matches;
            List<BlockStats> units = new();
            BlockStats unit;
            int depth = group.Depth + 1;
            string groupText = String.Empty;

            if (parent.CloseIndex > 0)
            {
                groupText = text.Substring(parent.OpenIndex, parent.CloseIndex - parent.OpenIndex);
            }


            // Create groups for each unit add to a list along with match start and block start
            // order units by match start
            // Start at end of parent block look for last unit, parent block end is its end
            // Look for next unit, previous match start is its block end
            // etc
            // Find all blocks within each unit, add them to unit
            // replace their parent with unit
            // repace group children with unit list
            // calculate all properties etc
            // calculate all depths
            // add units to blockscache
            foreach (GroupUnitSettings unitSettings in groupSettings.Units)
            {
                foreach (string pattern in unitSettings.Patterns)
                {
                    regex = new Regex(pattern);
                    matches = regex.Matches(groupText);

                    foreach (Match match in matches)
                    {
                        matchStart = parent.OpenIndex + match.Index;
                        matchEnd = matchStart + match.Value.Length;

                        unit = new(
                            group,
                            matchEnd,
                            depth,
                            new GroupUnitBoundarySettings(unitSettings.Name)
                        );

                        unit.MatchStart = matchStart;
                        unit.MatchEnd = matchEnd;
                        units.Add(unit);
                    }
                }
            }

            units.Sort(BlockStatsExtensions.Compare);
            int last = parent.CloseIndex;

            for (int i = units.Count - 1; i >= 0 ; i--)
            {
                unit = units[i];
                unit.CloseIndex = last;
                last = unit.MatchStart - 1;
            }

            for (int j = 0; j < units.Count; j++)
            {
                unit = units[j];

                for (int k = parent.ChildBlocks.Count - 1; k >= 0; k--)
                {
                    block = parent.ChildBlocks[k];

                    if (block.OpenIndex >= unit.OpenIndex
                        && block.CloseIndex <= unit.CloseIndex)
                    {
                        unit.ChildBlocks.Add(block);
                        block.Parent = unit;
                        parent.ChildBlocks.RemoveAt(k);
                    }
                }

                unit.BuildGroupUnitProperties(
                    boundaryCounter,
                    groupSettings,
                    text
                );

                blockStatsCache.BlockStats.Add(unit);
            }

            for (int i = 0; i < units.Count; i++)
            {
                group.ChildBlocks.Add(units[i]);
            }

            group.ResetDepth();
        }
    }
}
