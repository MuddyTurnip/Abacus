using Microsoft.ApplicationInspector.RulesEngine;
using System.Collections.Generic;

namespace MuddyTurnip.Metrics.Engine
{
    public class InlineCommentStripLoopCache
    {
        public List<Boundary> InlineBoundaries { get; }
        public int Adjustment { get; set; }
        public string InlineComment { get; set; }
        public SortedList<int, string> CommentContent { get; } = new();

        public InlineCommentStripLoopCache(
            List<Boundary> inlineBoundaries,
            string inlineComment)
        {
            InlineBoundaries = inlineBoundaries;
            InlineComment = inlineComment;
        }
    }
}
