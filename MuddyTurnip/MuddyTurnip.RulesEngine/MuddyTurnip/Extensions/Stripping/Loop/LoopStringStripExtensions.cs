﻿using Microsoft.ApplicationInspector.RulesEngine;
using MuddyTurnip.RulesEngine;
using MuddyTurnip.RulesEngine.Commands;
using System;
using System.Text;

namespace MuddyTurnip.Metrics.Engine
{
    public static class LoopStringStripExtensions
    {
        public static int ProcessString(
            this StringBuilder content,
            StringStripLoopCache cache,
            int index)
        {
            cache.StartNewString(
                content,
                index
            );

            if (cache.Current is { })
            {
                cache.Current.StringStarted = true;
            }

            int stringLength;
            char c;
            bool quoteEscaped;
            bool formatEscaped;
            int skipCount;
            int i = index + 1;

            for (; i < content.Length; i++)
            {
                c = content[i];

                if (c == cache.StringSettings.Quote)
                {
                    if (cache.Current?.StringStarted != true)
                    {
                        // Check whether the prefix is present - this deterines which escape character to search for before the next quote
                        cache.StartNewString(
                            content,
                            i
                        );
                    }
                    else
                    {
                        // Check to see if quote is escaped
                        (quoteEscaped, skipCount) = content.CheckIfQuoteCharEscaped(
                            cache,
                            i);

                        i += skipCount;

                        if (quoteEscaped)
                        {
                            continue;
                        }
                        else
                        {
                            stringLength = cache.End(
                                content,
                                i
                            );

                            i -= stringLength;

                            if (cache.Current is null)
                            {
                                break;
                            }
                        }
                    }
                }
                else if (cache.Current?.StringStarted == true
                        && cache.Current?.MultiLine == false
                        && c == '\n')
                {
                    stringLength = cache.End(
                        content,
                        i
                    );

                    i -= stringLength;

                    if (cache.Current is null)
                    {
                        break;
                    }
                }
                else if (cache.Current?.StringStarted == true
                        && cache.Current?.Format.Enabled == true
                        && c == cache.StringSettings.FormatPrefix)
                {
                    // Check to see if format prefix is escaped
                    (formatEscaped, skipCount) = content.CheckIfFormatPrefixEscaped(
                        cache,
                        i
                    );

                    i += skipCount;

                    if (formatEscaped)
                    {
                        continue;
                    }
                    else
                    {
                        // Pause string
                        stringLength = cache.Pause(
                            content,
                            i);

                        i -= stringLength;
                        cache.Current.Format.FormatStarted = true;
                        cache.Current.StringStarted = false;
                    }
                }
                else if (cache.Current?.StringStarted != true
                    && cache.Current?.Format.Enabled == true
                    && c == cache.StringSettings.FormatSuffix)
                {
                    // Check to see if format suffix is escaped
                    (formatEscaped, skipCount) = content.CheckIfFormatSuffixEscaped(
                        cache,
                        i
                    );

                    i += skipCount;

                    if (formatEscaped)
                    {
                        continue;
                    }
                    else
                    {
                        // Unpause string

                        if (cache.Current is null)
                        {
                            throw new ArgumentNullException(nameof(cache.Current));
                        }

                        cache.Current.StartIndex = i + 1;
                        cache.Current.StringStarted = true;
                    }
                }
            }

            return i;
        }

        private static int Pause(
            this StringStripLoopCache cache,
            StringBuilder content,
            int endStringIndex)
        {
            int stringLength = EndString(
                cache,
                content,
                endStringIndex
            );

            return stringLength;
        }

        private static int End(
            this StringStripLoopCache cache,
            StringBuilder content,
            int endStringIndex)
        {
            int stringLength = EndString(
                cache,
                content,
                endStringIndex
            );

            cache.Current = cache.Current?.Parent;

            return stringLength;
        }

        private static int EndString(
            this StringStripLoopCache cache,
            StringBuilder content,
            int endStringIndex)
        {
            if (cache.Current is null)
            {
                throw new ArgumentNullException(nameof(cache.Current));
            }

            int startStringIndex = cache.Current.StartIndex;

            // Reached the start of a string format
            // end the string
            int stringLength = endStringIndex - startStringIndex;
            int adjustedCommentStartIndex = startStringIndex + cache.OutputBoundaries.Adjustment;

            string value = content.ToString(
                startStringIndex,
                stringLength
            );

            cache.StringContent.AppendLine(value);

            // Remove line from content
            content.Remove(
                startStringIndex,
                stringLength
            );

            // Adjust the startStringIndex so it relates to the FullContent
            // not the content which might have had phrases removed
            MtBoundary stringBoundary = new MtBoundary(
                adjustedCommentStartIndex,
                stringLength,
                "codeString"
            );

            cache.OutputBoundaries.Boundaries.Add(stringBoundary);

            // Include the length of the removed string to the adjuster
            cache.OutputBoundaries.Adjustment += stringBoundary.Length;

            return stringLength;
        }

        private static void StartNewString(
            this StringStripLoopCache cache,
            StringBuilder content,
            int i)
        {
            StringEscape escape = cache.StringSettings.CharacterEscape;
            bool formatEnabled = false;
            bool multiLine = false;

            if (content[i - 1] == cache.StringSettings.PhrasePrefix)
            {
                escape = cache.StringSettings.PhraseCharacterEscape;
                multiLine = true;

                if (content[i - 2] == cache.StringSettings.FormatPhrasePrefix)
                {
                    formatEnabled = true;
                }
            }
            else if (content[i - 1] == cache.StringSettings.FormatPhrasePrefix)
            {
                formatEnabled = true;

                if (content[i - 2] == cache.StringSettings.PhrasePrefix)
                {
                    escape = cache.StringSettings.PhraseCharacterEscape;
                }
            }

            cache.Current = new StringState(
                escape,
                formatEnabled,
                cache.Current,
                i + 1,
                multiLine
            );
        }

        private static (bool escaped, int skipCount) CheckIfQuoteCharEscaped(
            this StringBuilder content,
            StringStripLoopCache cache,
            int i)
        {
            int skipCount = 0;

            if (cache.Current?.Escape.Location == "pre")
            {
                if (content[i - 1] == cache.Current?.Escape.Escape)
                {
                    return (true, skipCount);
                }
            }
            else if (cache.Current?.Escape.Location == "post")
            {
                if (content[i + 1] == cache.Current?.Escape.Escape)
                {
                    return (true, skipCount + 1);
                }
            }

            return (false, skipCount);
        }

        private static (bool escaped, int skipCount) CheckIfFormatPrefixEscaped(
            this StringBuilder content,
            StringStripLoopCache cache,
            int i)
        {
            int skipCount = 0;

            if (cache.StringSettings.FormatPrefixEscape.Location == "pre")
            {
                if (content[i - 1] == cache.StringSettings.FormatPrefixEscape.Escape)
                {
                    return (true, skipCount);
                }
            }
            else if (cache.StringSettings.FormatPrefixEscape.Location == "post")
            {
                if (content[i + 1] == cache.StringSettings.FormatPrefixEscape.Escape)
                {
                    return (true, skipCount + 1);
                }
            }

            return (false, skipCount);
        }

        private static (bool escaped, int skipCount) CheckIfFormatSuffixEscaped(
            this StringBuilder content,
            StringStripLoopCache cache,
            int i)
        {
            int skipCount = 0;

            if (cache.StringSettings.FormatSuffixEscape.Location == "pre")
            {
                if (content[i - 1] == cache.StringSettings.FormatSuffixEscape.Escape)
                {
                    return (true, skipCount);
                }
            }
            else if (cache.StringSettings.FormatSuffixEscape.Location == "post")
            {
                if (content[i + 1] == cache.StringSettings.FormatSuffixEscape.Escape)
                {
                    return (true, skipCount + 1);
                }
            }

            return (false, skipCount);
        }
    }
}
