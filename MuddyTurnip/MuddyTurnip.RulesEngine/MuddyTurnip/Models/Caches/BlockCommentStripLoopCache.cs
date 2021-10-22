using Microsoft.ApplicationInspector.RulesEngine;
using MuddyTurnip.RulesEngine;
using MuddyTurnip.RulesEngine.Commands;
using System.Collections.Generic;

namespace MuddyTurnip.Metrics.Engine
{
    public class BlockCommentStripLoopCache
    {
        public List<MtBoundary> InputBoundaries { get; }
        public List<MtBoundary> OutputBoundaries { get; }
        public int InputCounter { get; set; }
        public string PrefixComment { get; set; }
        public string SuffixComment { get; set; }
        public int InputAdjustment { get; set; }
        public int OutputAdjustment { get; set; }
        public SortedList<int, string> CommentContent { get; }
        public List<BlockStatsError> Errors { get; set; } = new();

        public BlockCommentStripLoopCache(
            List<MtBoundary> inputBoundaries,
            List<MtBoundary> outputBoundaries,
            string prefixComment,
            string suffixComment,
            SortedList<int, string> commentContent)
        {
            InputBoundaries = inputBoundaries;
            OutputBoundaries = outputBoundaries;
            PrefixComment = prefixComment;
            SuffixComment = suffixComment;
            CommentContent = commentContent;
        }
    }
}
