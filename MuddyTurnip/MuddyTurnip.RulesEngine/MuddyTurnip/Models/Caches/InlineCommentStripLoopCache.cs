using Microsoft.ApplicationInspector.RulesEngine;
using MuddyTurnip.RulesEngine;
using MuddyTurnip.RulesEngine.Commands;
using System.Collections.Generic;

namespace MuddyTurnip.Metrics.Engine
{
    public class InlineCommentStripLoopCache
    {
        public List<MtBoundary> InlineBoundaries { get; }
        public int Adjustment { get; set; }
        public string InlineComment { get; set; }
        public StringSettings StringSettings { get; set; }
        public CommentSettings CommentSettings { get; set; }
        public SortedList<int, string> CommentContent { get; } = new();

        public InlineCommentStripLoopCache(
            List<MtBoundary> inlineBoundaries,
            string inlineComment,
            StringSettings stringSettinge,
            CommentSettings commentSettings)

        {
            InlineBoundaries = inlineBoundaries;
            InlineComment = inlineComment;
            StringSettings = stringSettinge;
            CommentSettings = commentSettings;
        }
    }
}
