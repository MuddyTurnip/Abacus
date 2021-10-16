using Microsoft.ApplicationInspector.RulesEngine;
using System.Collections.Generic;

namespace MuddyTurnip.Metrics.Engine
{
    public class BlockCommentStripLoopCache
    {
        public List<Boundary> InputBoundaries { get; }
        public List<Boundary> OutputBoundaries { get; }
        public int InputCounter { get; set; }
        public string PrefixComment { get; set; }
        public string SuffixComment { get; set; }
        public int InputAdjustment { get; set; }
        public int OutputAdjustment { get; set; }
        public SortedList<int, string> CommentContent { get; }

        public BlockCommentStripLoopCache(
            List<Boundary> inputBoundaries,
            List<Boundary> outputBoundaries,
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
