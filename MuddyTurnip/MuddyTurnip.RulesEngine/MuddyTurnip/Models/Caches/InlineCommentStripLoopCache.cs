using Microsoft.ApplicationInspector.RulesEngine;
using MuddyTurnip.RulesEngine;
using MuddyTurnip.RulesEngine.Commands;
using System.Collections.Generic;

namespace MuddyTurnip.Metrics.Engine
{
    public class InlineCommentStripLoopCache
    {
        //public int Adjustment { get; set; }
        public string InlineComment { get; set; }
        public List<CommentBoundary> Comments { get; } = new();
        public OutputBoundaries OutputBoundaries { get; }

        public InlineCommentStripLoopCache(
            string inlineComment,
            OutputBoundaries outputBoundaries)
        {
            InlineComment = inlineComment;
            OutputBoundaries = outputBoundaries;
        }
    }
}
