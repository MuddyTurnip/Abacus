using Microsoft.ApplicationInspector.RulesEngine;
using System.Collections.Generic;
using System.Diagnostics;

namespace MuddyTurnip.RulesEngine
{
    [DebuggerDisplay("({BlockStartLocation.Line}-{BlockStartLocation.Column}) => ({BlockEndLocation.Line}-{BlockEndLocation.Column}) - {Settings.Model} -{Settings.BlockType} - {Name} - {Type}")]
    public class BlockStats
    {
        private int _closeIndex;

        public string Name { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public BlockStatsError? Error { get; set; } = null;
        public string Signature { get; set; } = string.Empty;
        public string CleanedSignature { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string StrippedContent { get; set; } = string.Empty;
        public IBoundarySettings Settings { get; set; }
        public int MatchStart { get; set; }
        public int MatchEnd { get; set; }
        public int AdjustedMatchStart { get; set; }
        public int AdjustedMatchEnd { get; set; }
        public Location MatchStartLocation { get; set; } = new();
        public Location MatchEndLocation { get; set; } = new();
        public List<KeyValuePair<string, string>> PropertyPairs { get; } = new();
        public List<string> Flags { get; } = new();
        public bool QualifiesName { get; set; } = false;
        public int Depth { get; set; }
        public int SyblingCount { get; set; }
        public int OpenIndex { get; set; }
        public int AdjustedOpenIndex { get; set; }
        public int AdjustedCloseIndex { get; set; }
        public Location BlockStartLocation { get; set; } = new();
        public Location BlockEndLocation { get; set; } = new();
        public List<BlockStats> ChildBlocks { get; set; } = new();
        public List<StatementStats> ChildStatements { get; set; } = new();
        public BlockStats? Parent { get; set; }

        public int CloseIndex {
            get => _closeIndex;
            set
            {
                _closeIndex = value;

                if (_closeIndex < OpenIndex)
                {
                    Error = new BlockStatsError("Unclosed block", "There is a block without a closeIndex, this could corrupt all the results.");
                }
            }
        }

        public BlockStats(
            BlockStats? parent,
            int openIndex,
            int depth,
            IBoundarySettings settings)
        {
            Parent = parent;
            OpenIndex = openIndex;
            Depth = depth;
            Settings = settings;
        }
    }
}