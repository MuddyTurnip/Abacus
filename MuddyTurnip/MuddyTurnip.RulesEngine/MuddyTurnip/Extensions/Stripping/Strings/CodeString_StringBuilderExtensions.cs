﻿using Microsoft.ApplicationInspector.RulesEngine;
using MuddyTurnip.RulesEngine;
using MuddyTurnip.RulesEngine.Commands;
using System;
using System.Text;

namespace MuddyTurnip.Metrics.Engine
{
    public static class CodeString_StringBuilderExtensions
    {
        public static void StripStrings(
            this StringBuilder content,
            StringStripLoopCache cache)
        {
            // Start at zero then look for "
            // The character after is the start of the string
            // then check to see if it is preceeded by @ 
            // if so use phrasecharacterescape
            // if not use the characterescape
            // then check for the next "
            // check caracter before to if it is escaped
            // if it is ignore and search for next " or {
            // if it isn't escaped then the preceeding character is the end of the string
            // remove the string leaving the first " and the last "
            // then look for next "

            int startStringIndex = 0;
            int stringLength;
            char c;
            //StringEscape? quoteEscape = null;
            bool quoteEscaped;
            //bool formatEnabled = false;
            bool formatEscaped;
            int skipCount;
            //bool stringStarted = false;
            //bool formatStarted = false;

            for (int i = startStringIndex; i < content.Length; i++)
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
                        cache.Current.StringStarted = false;
                        cache.Current.StringStarted = true;
                    }
                }
            }

            cache.CompleteMergeInBoundaries();
        }

        private static void MergeInBoundary(
            this StringStripLoopCache cache,
            MtBoundary stringBoundary)
        {
            MtBoundary inputBoundary;
            int adjustedIndex;

            for (int i = cache.InputCounter; i < cache.InputBoundaries.Count; i++)
            {
                inputBoundary = cache.InputBoundaries[i];
                cache.InputCounter = i;
                adjustedIndex = stringBoundary.Index + cache.OutputAdjustment + cache.InputAdjustment;

                if (inputBoundary.Index > adjustedIndex)
                {
                    break;
                }

                cache.OutputBoundaries.Add(inputBoundary);
                cache.InputAdjustment += inputBoundary.Length;
                cache.InputCounter++;

                if (inputBoundary.Index == stringBoundary.Index)
                {
                    break;
                }
            }

            cache.AddStringBoundary(stringBoundary);
        }

        public static void CompleteMergeInBoundaries(this StringStripLoopCache cache)
        {
            int i = cache.InputCounter;

            for (; i < cache.InputBoundaries.Count; i++)
            {
                cache.OutputBoundaries.Add(cache.InputBoundaries[i]);
            }
        }

        public static int Pause(
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

        public static int End(
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

        public static int EndString(
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
                startStringIndex,
                stringLength,
                "codeString"
            );

            cache.MergeInBoundary(stringBoundary);

            // Include the length of the removed string to the adjuster
            cache.OutputAdjustment += stringBoundary.Length;

            return stringLength;
        }

        public static void AddStringBoundary(
            this StringStripLoopCache cache,
            MtBoundary blockBoundary)
        {
            blockBoundary.Index += cache.InputAdjustment + cache.OutputAdjustment;
            cache.OutputBoundaries.Add(blockBoundary);
        }

        public static void StartNewString(
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

        public static (bool escaped, int skipCount) CheckIfQuoteCharEscaped(
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

        public static (bool escaped, int skipCount) CheckIfFormatPrefixEscaped(
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

        public static (bool escaped, int skipCount) CheckIfFormatSuffixEscaped(
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
