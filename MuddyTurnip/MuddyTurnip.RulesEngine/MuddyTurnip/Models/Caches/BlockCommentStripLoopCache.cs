using Microsoft.ApplicationInspector.RulesEngine;
using MuddyTurnip.RulesEngine;
using MuddyTurnip.RulesEngine.Commands;
using System.Collections.Generic;

namespace MuddyTurnip.Metrics.Engine
{
    public class BlockCommentStripLoopCache
    {
        public int InputCounter { get; set; }
        public string PrefixComment { get; set; }
        public string SuffixComment { get; set; }
        //public int InputAdjustment { get; set; }
        //public int OutputAdjustment { get; set; }
        public List<CommentBoundary> Comments { get; } = new();
        public List<BlockStatsError> Errors { get; set; } = new();
        public OutputBoundaries OutputBoundaries { get; }

        public BlockCommentStripLoopCache(
            string prefixComment,
            string suffixComment,
            List<CommentBoundary> comments,
            OutputBoundaries outputBoundaries)
        {
            PrefixComment = prefixComment;
            SuffixComment = suffixComment;
            Comments = comments;
            OutputBoundaries = outputBoundaries;
        }
    }
}
