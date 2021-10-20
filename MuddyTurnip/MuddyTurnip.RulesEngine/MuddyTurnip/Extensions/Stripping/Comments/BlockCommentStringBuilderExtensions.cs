using Microsoft.ApplicationInspector.RulesEngine;
using MuddyTurnip.RulesEngine.Commands;
using System.Text;

namespace MuddyTurnip.Metrics.Engine
{
    static class BlockCommentStringBuilderExtensions
    {
        public static void StripBlockComments(
            this StringBuilder content,
            BlockCommentStripLoopCache cache)
        {
            int suffixLength = cache.SuffixComment.Length;
            int prefixLength = cache.PrefixComment.Length;
            int commentStartIndex = content.IndexOf(cache.PrefixComment, 0);
            int commentEndIndex;
            int commentLength;
            int adjustedIndex;
            string comment;
            int startSearchIndex = commentStartIndex + prefixLength;
            MtBoundary blockBoundary;

            while (commentStartIndex > -1)
            {
                commentEndIndex = content.IndexOf(
                    cache.SuffixComment,
                    startSearchIndex
                );

                commentLength = commentEndIndex + suffixLength - commentStartIndex;

                comment = content.ToString(
                    commentStartIndex,
                    commentLength
                );

                // Remove comment from content
                content.Remove(
                    commentStartIndex,
                    commentLength
                );

                blockBoundary = new MtBoundary(
                    commentStartIndex,
                    commentLength,
                    "blockComment"
                );

                cache.MergeInBoundary(blockBoundary);

                adjustedIndex = AdjustIndexNumber(
                    cache,
                    commentStartIndex
                );

                cache.CommentContent.Add(
                    (adjustedIndex * 10) + 1,
                    comment
                );

                // Include the length of the removed string to the adjuster
                cache.OutputAdjustment += blockBoundary.Length;

                startSearchIndex = startSearchIndex < 0
                    ? 0
                    : startSearchIndex;

                // Look for the next block comment prefix
                commentStartIndex = content.IndexOf(
                    cache.PrefixComment,
                    startSearchIndex);

                // Set the next search to start where this comment started (it will be removed in next loop) 
                startSearchIndex = commentStartIndex;

                if (startSearchIndex < 0)
                {
                    break;
                }
            }

            cache.CompleteMergeInBoundaries();
        }

        private static void MergeInBoundary(
            this BlockCommentStripLoopCache cache,
            MtBoundary blockBoundary)
        {
            int i = cache.InputCounter;
            MtBoundary inlineBoundary;
            int adjustedIndex;

            for (; i < cache.InputBoundaries.Count; i++)
            {
                inlineBoundary = cache.InputBoundaries[i];
                cache.InputCounter = i;
                adjustedIndex = blockBoundary.Index + cache.OutputAdjustment + cache.InputAdjustment;

                if (inlineBoundary.Index > adjustedIndex)
                {
                    break;
                }

                cache.OutputBoundaries.Add(inlineBoundary);
                cache.InputAdjustment += inlineBoundary.Length;
                cache.InputCounter++;

                if (inlineBoundary.Index == blockBoundary.Index)
                {
                    break;
                }
            }

            cache.AddBlockBoundary(blockBoundary);
        }

        public static void CompleteMergeInBoundaries(this BlockCommentStripLoopCache cache)
        {
            int i = cache.InputCounter;

            for (; i < cache.InputBoundaries.Count; i++)
            {
                cache.OutputBoundaries.Add(cache.InputBoundaries[i]);
            }
        }

        public static void AddBlockBoundary(
            this BlockCommentStripLoopCache cache,
            MtBoundary blockBoundary)
        {
            blockBoundary.Index += cache.InputAdjustment + cache.OutputAdjustment;
            cache.OutputBoundaries.Add(blockBoundary);
        }

        public static int AdjustIndexNumber(
            this BlockCommentStripLoopCache cache,
            int strippedIndex)
        {
            MtBoundary inputBoundary;
            int adjustedIndex = strippedIndex;

            for (int i = 0; i < cache.OutputBoundaries.Count; i++)
            {
                inputBoundary = cache.OutputBoundaries[i];

                if (inputBoundary.Index > adjustedIndex)
                {
                    return adjustedIndex;
                }

                adjustedIndex += inputBoundary.Length;
            }

            return adjustedIndex;
        }
    }
}
