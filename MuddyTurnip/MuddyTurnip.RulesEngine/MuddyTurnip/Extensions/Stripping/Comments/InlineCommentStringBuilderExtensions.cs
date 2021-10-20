using Microsoft.ApplicationInspector.RulesEngine;
using MuddyTurnip.RulesEngine.Commands;
using System.Text;

namespace MuddyTurnip.Metrics.Engine
{
    static class InlineCommentStringBuilderExtensions
    {
        public static void StripInlineComments(
            this StringBuilder content,
            InlineCommentStripLoopCache cache)
        {
            int commentStartIndex = content.IndexOf(
                cache.InlineComment, 
                0
            );

            int commentEndIndex;
            int commentLength;
            string comment;
            int adjustedCommentStartIndex;
            int startSearchIndex = commentStartIndex;
            MtBoundary inlineBoundary;

            while (commentStartIndex > -1)
            {
                commentEndIndex = content.IndexOf(
                    '\n', 
                    commentStartIndex);

                if (commentEndIndex < 0)
                {
                    break;
                }

                commentLength = commentEndIndex - commentStartIndex;
                adjustedCommentStartIndex = commentStartIndex + cache.Adjustment;

                comment = content.ToString(
                    commentStartIndex, 
                    commentLength
                );

                // Remove comment from content
                content.Remove(
                    commentStartIndex,
                    commentLength
                );

                inlineBoundary = new MtBoundary(
                    adjustedCommentStartIndex,
                    commentLength,
                    "inlineComment"
                );

                cache.InlineBoundaries.Add(inlineBoundary);

                cache.CommentContent.Add(
                    (adjustedCommentStartIndex * 10), 
                    comment
                );

                // Include the length of the removed string to the adjuster
                cache.Adjustment += commentLength;

                startSearchIndex = startSearchIndex < 0 
                    ? 0 
                    : startSearchIndex;

                // Look for the next inline comment
                commentStartIndex = content.IndexOf(
                    cache.InlineComment, 
                    startSearchIndex);

                // Set the next search to start where this comment started (it will be removed in next loop) 
                startSearchIndex = commentStartIndex;

                if (startSearchIndex < 0)
                {
                    break;
                }
            }
        }
    }
}
