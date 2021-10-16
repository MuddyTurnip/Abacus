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
        public int NextGroupID => ++_nextGroupID;

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
