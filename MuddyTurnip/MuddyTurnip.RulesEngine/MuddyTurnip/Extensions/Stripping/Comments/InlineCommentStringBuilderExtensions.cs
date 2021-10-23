﻿using Microsoft.ApplicationInspector.RulesEngine;
using MuddyTurnip.RulesEngine.Commands;
using System.Collections.Generic;
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
            bool isShieldedByQuote;

            while (commentStartIndex > -1)
            {
                // Is comment preceded by a single line quote
                // or  
                isShieldedByQuote = content.CheckQuotes(
                    cache,
                    commentStartIndex
                );

                if (isShieldedByQuote)
                {
                    // Look for the next inline comment
                    startSearchIndex += cache.InlineComment.Length;

                    commentStartIndex = content.IndexOf(
                        cache.InlineComment,
                        startSearchIndex);

                    continue;
                }

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

        private static bool CheckQuotes(
            this StringBuilder content,
            InlineCommentStripLoopCache cache,
            int commentStartIndex)
        {
            int inlineCommentEndIndex = commentStartIndex + cache.InlineComment.Length;
            int stopIndex = inlineCommentEndIndex > content.Length ? content.Length : inlineCommentEndIndex;

            List <MtBoundary> emotyBoundaries = new();
            List<MtBoundary> stringBoundaries = new();
            StringBuilder copiedContent = new StringBuilder(content.ToString());
            StringBuilder stringContent = new StringBuilder();

            StringStripLoopCache stringCache = new(
                emotyBoundaries,
                stringBoundaries,
                stringContent,
                cache.StringSettings
            );

            copiedContent.LocateStrings(
                stringCache,
                stopIndex)
            ;

            foreach (MtBoundary stringBoundary in stringCache.OutputBoundaries)
            {
                if (stringBoundary.Index <= commentStartIndex
                    && stringBoundary.Index + stringBoundary.Length >= inlineCommentEndIndex)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
