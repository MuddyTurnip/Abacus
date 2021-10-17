// Copyright (C) Microsoft. All rights reserved. Licensed under the MIT License.

using Microsoft.ApplicationInspector.RulesEngine;
using MuddyTurnip.Metrics.Engine;
using MuddyTurnip.RulesEngine.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace MuddyTurnip.RulesEngine
{
    /// <summary>
    ///     Class to handle text as a searchable container
    /// </summary>
    public class BlockTextContainer : IBoundaryCounter
    {
        private CommentSettings _commentSettings;
        private StringSettings _stringSettings;
        private CodeBlockSettings _codeBlockSettings;
        private PreProcessorSettings _preProcessorSettings;
        private FileStructureSettings _fileStructureSettings;

        private FileStructureFactory _fileStructureFactory;
        private IFileStructureInterpreter _fileStructureProcessor;

        private StringBuilder _strippedContent;
        private StringBuilder _commentContent;
        private StringBuilder _preProcessorContent;
        private StringBuilder _stringContent;

        private List<Boundary> _outputBoundaries;
        private BlockStatsCache? _blockStatsCache;

        private string _fullContent;
        public BlockStatsCache? BlockStatsCache => _blockStatsCache;
        public string RawContent => _fullContent;
        public string CodeContent => _strippedContent.ToString();
        public string CommentContent => _commentContent.ToString();
        public string PreProcessorContent => _preProcessorContent.ToString();
        public string StringContent => _stringContent.ToString();
        public string Language { get; }
        public string Line { get; } = "";
        public int LineNumber { get; }
        public List<int> LineStarts { get; }
        public List<int> LineEnds { get; }
        //public string Target { get; }

        /// <summary>
        ///     Creates new instance
        /// </summary>
        /// <param name="content"> Text to work with </param>
        /// <param name="language"> The language of the test </param>
        /// <param name="lineNumber"> The line number to specify. Leave empty for full file as target. </param>
        public BlockTextContainer(
            string content,
            string language,
            int lineNumber = 0,
            bool setBlockContent = false)
        {
            if (lineNumber != 0)
            {
                throw new NotImplementedException("Line number was not zero - debug to understand the events that led to this happening.");
            }

            Language = language;
            LineNumber = lineNumber;
            _fullContent = content;
            //Target = LineNumber == 0 ? _fullContent : GetLineContent(lineNumber);
            LineEnds = new List<int>() { 0 };
            LineStarts = new List<int>() { 0, 0 };

            _strippedContent = new StringBuilder(content);
            _commentContent = new StringBuilder();
            _preProcessorContent = new StringBuilder();
            _stringContent = new StringBuilder();

            _fileStructureFactory = new();
            _commentSettings = MtLanguage.GetCommentSettings(Language);
            _stringSettings = MtLanguage.GetStringSettings(Language);
            _codeBlockSettings = MtLanguage.GetCodeBlockSettings(Language);
            _preProcessorSettings = MtLanguage.GetPreProcessorSettings(Language);
            _fileStructureSettings = MtLanguage.GetStructureSettings(Language);

            _fileStructureProcessor = _fileStructureFactory.Build(_fileStructureSettings);

            // Find line end in the text
            int pos = 0;

            while (pos > -1 && pos < _fullContent.Length)
            {
                if (++pos < _fullContent.Length)
                {
                    pos = _fullContent.IndexOf('\n', pos);
                    LineEnds.Add(pos);

                    if (pos > 0 && pos + 1 < _fullContent.Length)
                    {
                        LineStarts.Add(pos + 1);
                    }
                }
            }

            // Text can end with \n or not
            if (LineEnds[LineEnds.Count - 1] == -1)
            {
                LineEnds[LineEnds.Count - 1] = (_fullContent.Length > 0) ? _fullContent.Length - 1 : 0;
            }

            // Run these before adding last EndLine if it doesn't end in a new line...
            List<Boundary> inlineBoundaries = new();
            List<Boundary> commentBoundaries = new();

            StripComments(
                inlineBoundaries,
                commentBoundaries
            );

            List<Boundary> preProcessorBoundaries = new();

            StripPreProcessors(
                commentBoundaries,
                preProcessorBoundaries
            );

            List<Boundary> blankLineBoundaries = new();

            StripBlankLines(
                preProcessorBoundaries,
                blankLineBoundaries);

            List<Boundary> stringBoundaries = new();

            StripStrings(
                blankLineBoundaries,
                stringBoundaries
            );

            _outputBoundaries = stringBoundaries;

            CodeBlockLoopCache codeBlockLoopCache = CountCodeBlocks();

            StructureFile(
                codeBlockLoopCache,
                setBlockContent
            );
        }

        public string GetTarget(MtPatternScope[] scopes)
        {
            if (scopes.Length == 1)
            {
                if (scopes.Contains(MtPatternScope.Code))
                {
                    return CodeContent;
                }
                else if (scopes.Contains(MtPatternScope.Comment))
                {
                    return CommentContent;
                }
            }

            return _fullContent;
        }

        /// <summary>
        ///     Returns the Boundary of a specified line number
        /// </summary>
        /// <param name="lineNumber"> The line number to return the boundary for </param>
        /// <returns> </returns>
        public Boundary GetBoundaryFromLine(int lineNumber)
        {
            Boundary result = new Boundary();

            if (lineNumber >= LineEnds.Count)
            {
                return result;
            }

            // Fine when the line number is 0
            var start = 0;

            if (lineNumber > 0)
            {
                start = LineEnds[lineNumber - 1] + 1;
            }

            result.Index = start;
            result.Length = LineEnds[lineNumber] - result.Index + 1;

            return result;
        }

        public int GetFullIndexFromCodeIndex(int codeIndex)
        {
            Boundary inputBoundary;
            int fullIndex = codeIndex;

            for (int i = 0; i < _outputBoundaries.Count; i++)
            {
                inputBoundary = _outputBoundaries[i];

                if (inputBoundary.Index > fullIndex)
                {
                    return fullIndex;
                }

                fullIndex += inputBoundary.Length;
            }

            return fullIndex;
        }

        public string GetBoundaryText(Boundary capture)
        {
            if (capture is null)
            {
                return string.Empty;
            }

            int contentStart = Math.Min(_fullContent.Length, capture.Index);
            int contentEnd = Math.Min(_fullContent.Length, capture.Index + capture.Length);

            return _fullContent[contentStart..contentEnd];
        }

        /// <summary>
        ///     Returns boundary for a given index in text
        /// </summary>
        /// <param name="index"> Position in text </param>
        /// <returns> Boundary </returns>
        public Boundary GetLineBoundary(
            int index,
            MtPatternScope[] scopes)
        {
            Boundary result = new Boundary();

            for (int i = 0; i < LineEnds.Count; i++)
            {
                if (LineEnds[i] >= index)
                {
                    result.Index = (i > 0 && LineEnds[i - 1] > 0) ? LineEnds[i - 1] + 1 : 0;
                    result.Length = LineEnds[i] - result.Index + 1;

                    break;
                }
            }

            return result;
        }

        public void StructureFile(
             CodeBlockLoopCache codeBlockLoopCache,
             bool setBlockContent)
        {
            BlockStatsCache blockStatsCache = new BlockStatsCache(
                _fileStructureSettings,
                codeBlockLoopCache.CodeBlockSettings,
                codeBlockLoopCache.RootCodeBlock,
                codeBlockLoopCache.BlockStats
            );

            _fileStructureProcessor.StructureFile(
                blockStatsCache,
                CodeContent,
                this,
                setBlockContent,
                _fullContent
            );

            _blockStatsCache = blockStatsCache;
        }

        public CodeBlockLoopCache CountCodeBlocks()
        {
            BlockStats root = new(
                null,
                0,
                0,
                new BlockBoundarySettings(
                    "file",
                    "file",
                    String.Empty,
                    String.Empty,
                    String.Empty
                )
            );

            root.CloseIndex = _strippedContent.Length - 1;

            CodeBlockLoopCache codeBlockLoopCache = new(
                root,
                _codeBlockSettings);

            _strippedContent.CountCodeBlocks(
                codeBlockLoopCache,
                this);

            return codeBlockLoopCache;
        }

        public void StripComments(
            List<Boundary> inlineBoundaries,
            List<Boundary> allCommentBoundaries)
        {
            InlineCommentStripLoopCache inlineCache = new(
                inlineBoundaries,
                _commentSettings.Inline);

            _strippedContent.StripInlineComments(inlineCache);

            BlockCommentStripLoopCache blockCache = new(
                inlineBoundaries,
                allCommentBoundaries,
                _commentSettings.Prefix,
                _commentSettings.Suffix,
                inlineCache.CommentContent);

            _strippedContent.StripBlockComments(blockCache);

            foreach (KeyValuePair<int, string> comment in blockCache.CommentContent)
            {
                _commentContent.AppendLine(comment.Value);
            }
        }

        public void StripPreProcessors(
            List<Boundary> inputBoundaries,
            List<Boundary> outputBoundaries)
        {
            PreProcessorStripLoopCache cache = new(
                inputBoundaries,
                outputBoundaries,
                _preProcessorContent,
                _preProcessorSettings.Preprocessors.ToArray());

            if (_preProcessorSettings.Preprocessors.Count == 1)
            {
                _strippedContent.SingleStripPreProcessors(cache);
            }
            else
            {
                _strippedContent.MultiStripPreProcessors(cache);
            }
        }

        public void StripBlankLines(
            List<Boundary> commentBoundaries,
            List<Boundary> allBoundaries)
        {
            BlankLineStripLoopCache blankLineCache = new(
                commentBoundaries,
                allBoundaries);

            _strippedContent.StripBlankLines(blankLineCache);
        }

        public void StripStrings(
            List<Boundary> blankLineBoundaries,
            List<Boundary> allBoundaries)
        {
            StringStripLoopCache stringCache = new(
                blankLineBoundaries,
                allBoundaries,
                _stringContent,
                _stringSettings
            );

            _strippedContent.StripStrings(stringCache);
        }

        /// <summary>
        ///     Return content of the line
        /// </summary>
        /// <param name="line"> Line number </param>
        /// <returns> Text </returns>
        public string GetLineContent(
            int line,
            MtPatternScope[] scopes)
        {
            int index = LineEnds[line];

            Boundary bound = GetLineBoundary(
                index,
                scopes
            );

            return _fullContent.Substring(bound.Index, bound.Length);
        }

        /// <summary>
        ///     Returns location (Line, Column) for given index in text
        /// </summary>
        /// <param name="index"> Position in text </param>
        /// <returns> Location </returns>
        public Location GetLocation(int index)
        {
            Location result = new Location();
            bool matched = false;

            if (index == 0)
            {
                result.Line = 1;
                result.Column = 1;
                matched = true;
            }
            else
            {
                for (int i = 0; i < LineEnds.Count; i++)
                {
                    if (LineEnds[i] >= index)
                    {
                        result.Line = i;
                        result.Column = index - LineEnds[i - 1];
                        matched = true;

                        break;
                    }
                }
            }

            if (!matched)
            {
                throw new NotImplementedException($"Could not find a location for the index: {index}");
            }

            return result;
        }

        /// <summary>
        ///     Check whether the boundary in a text matches the scope of a search pattern(code, comment etc.)
        /// </summary>
        /// <param name = "pattern" > Pattern with scope</param>
        /// <param name = "boundary" > Boundary in a text </param>
        /// <param name = "text" > Text </ param >
   
        //   / < returns > True if boundary is matching the pattern scope </returns>
        //public bool ScopeMatch(
        //    IEnumerable<PatternScope> patterns,
        //    Boundary boundary)
        //{
        //    if (patterns is null)
        //    {
        //        return true;
        //    }

        //    if (patterns.Contains(PatternScope.All)
        //        || string.IsNullOrEmpty(_commentSettings.Prefix))
        //    {
        //        return true;
        //    }

        //    bool isInComment = IsBetween(
        //        _fullContent,
        //        boundary.Index,
        //        _commentSettings.Prefix,
        //        _commentSettings.Suffix,
        //        _commentSettings.Inline);

        //    return !(isInComment && !patterns.Contains(PatternScope.Comment));
        //}

        /// <summary>
        ///     Return boundary defined by line and its offset
        /// </summary>
        /// <param name="line"> Line number </param>
        /// <param name="offset"> Offset from line number </param>
        /// <returns> Boundary </returns>
        private int BoundaryByLine(
            int line,
            int offset)
        {
            int index = line + offset;

            // We need the begining of the line when going up
            if (offset < 0)
            {
                index--;
            }

            if (index < 0)
            {
                index = 0;
            }

            if (index >= LineEnds.Count)
            {
                index = LineEnds.Count - 1;
            }

            return LineEnds[index];
        }

        /// <summary>
        ///     Checks if the index in the string lies between preffix and suffix
        /// </summary>
        /// <param name="text"> Text </param>
        /// <param name="index"> Index to check </param>
        /// <param name="prefix"> Prefix </param>
        /// <param name="suffix"> Suffix </param>
        /// <returns> True if the index is between prefix and suffix </returns>
        private static bool IsBetween(
            string text,
            int index,
            string prefix,
            string suffix,
            string inline = "")
        {
            int pinnedIndex = Math.Min(
                index,
                text.Length);

            string preText = string.Concat(text.Substring(0, pinnedIndex));

            int lastPrefix = preText.LastIndexOf(
                prefix,
                StringComparison.InvariantCulture);

            if (lastPrefix >= 0)
            {
                int lastInline = preText
                    .Substring(0, lastPrefix)
                    .LastIndexOf(inline, StringComparison.InvariantCulture);

                // For example in C#, If this /* is actually commented out by a //
                if (!(lastInline >= 0
                    && lastInline < lastPrefix
                    && !preText
                        .Substring(lastInline, lastPrefix - lastInline)
                        .Contains('\n')))
                {
                    var commentedText = text.Substring(lastPrefix);

                    int nextSuffix = commentedText.IndexOf(
                        suffix,
                        StringComparison.InvariantCulture
                    );

                    // If the index is in between the last prefix before the index and the next suffix after
                    // that prefix Then it is commented out
                    if (lastPrefix + nextSuffix > pinnedIndex)
                    {
                        return true;
                    }
                }
            }

            if (!string.IsNullOrEmpty(inline))
            {
                int lastInline = preText.LastIndexOf(
                    inline,
                    StringComparison.InvariantCulture
                );

                if (lastInline >= 0)
                {
                    //extra check to ensure inline is not part of a file path or url i.e. http://111.333.44.444
                    if (lastInline > 1)
                    {
                        if (text[lastInline - 1] != ' ') //indicates not an actual inline comment
                        {
                            return false;
                        }
                    }

                    var commentedText = text.Substring(lastInline);
                    int endOfLine = commentedText.IndexOf('\n');//Environment.Newline looks for /r/n which is not guaranteed

                    if (endOfLine < 0)
                    {
                        endOfLine = commentedText.Length - 1;
                    }
                    if (index > lastInline
                        && index < lastInline + endOfLine)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
