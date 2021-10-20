using Microsoft.ApplicationInspector.RulesEngine;
using MuddyTurnip.RulesEngine;
using System.Collections.Generic;

namespace MuddyTurnip.Metrics.Engine
{
    public class BlockStatsCache
    {
        private int _nextGroupID = 0;
        public FileStructureSettings FileStructureSettings { get; set; }
        public CodeBlockSettings CodeBlockSettings { get; }
        public BlockStats RootBlockStats { get; }
        public List<BlockStats> BlockStats { get; }
        public List<LocationIndex> UnitsOfWorkStart { get; } = new();

        public BlockStatsCache(
            FileStructureSettings fileStructureSettings,
            CodeBlockSettings codeBlockSettings,
            BlockStats rootBlockStats,
            List<BlockStats> blockStats)
        {
            FileStructureSettings = fileStructureSettings;
            CodeBlockSettings = codeBlockSettings;
            RootBlockStats = rootBlockStats;
            BlockStats = blockStats;
        }
    }
}
