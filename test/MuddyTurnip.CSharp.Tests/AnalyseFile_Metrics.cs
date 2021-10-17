using Microsoft.ApplicationInspector.Commands;
using MuddyTurnip.Metrics.Engine;
using MuddyTurnip.RulesEngine.Commands;
using System;
using System.Collections.Generic;
using Xunit;

namespace MuddyTurnip.RegularExpression.Tests
{
    public class AnalyseFile_Metrics
    {
        public MtAnalyzeResult? _analyzeResult = null;
        public AnalyseFile_Metrics()
        {
            MtAnalyzeCommand command = new MtAnalyzeCommand(new AnalyzeOptions()
            {
                SourcePath = new String[] { "E:\\Delete\\ApplicationInspector\\Test" },
                CustomRulesPath = "C:\\GitHub\\Abacus\\MuddyTurnip\\MuddyTurnip.Abacus\\CodemologyRules\\",
                IgnoreDefaultRules = true,
                ConfidenceFilters = "high,medium",
                FilePathExclusions = new String[] { "**/bin/**", "**/obj/**", "**/.vs/**", "**/.git/**", "*.json" },
                ConsoleVerbosityLevel = "medium",
                //Log = cliOptions.Log,
                SingleThread = true,
                NoShowProgress = false,
                FileTimeOut = 60000,
                ProcessingTimeOut = 0,
                ContextLines = 3,
                ScanUnknownTypes = false,
                TagsOnly = false,
                NoFileMetadata = false
            }); ;

            _analyzeResult = command.GetResult();
        }

        [Fact]
        public void CheckMetricCounts()
        {
            Assert.Equal(MtAnalyzeResult.ExitCode.Success, _analyzeResult?.ResultCode);
            Assert.Equal(1, _analyzeResult?.Metadata.AbacusRecords.Count);

            AbacusRecord? abacusRecord = _analyzeResult?.Metadata.AbacusRecords[0];
            Assert.NotNull(abacusRecord);

            Assert.Equal(29, GetTagCount(abacusRecord?.TagCounts, "Codemology-m.Code.CSharp.string"));
            Assert.Equal(10, GetTagCount(abacusRecord?.TagCounts, "Codemology-m.Code.CSharp.return"));
            Assert.Equal(9, GetTagCount(abacusRecord?.TagCounts, "Codemology-m.Code.CSharp.new"));
            Assert.Equal(10, GetTagCount(abacusRecord?.TagCounts, "Codemology-m.Code.CSharp.false"));
            Assert.Equal(10, GetTagCount(abacusRecord?.TagCounts, "Codemology-m.Code.CSharp.internal"));
            Assert.Equal(5, GetTagCount(abacusRecord?.TagCounts, "Codemology-m.Code.CSharp.if"));
            Assert.Equal(2, GetTagCount(abacusRecord?.TagCounts, "Codemology-m.Code.CSharp.throw"));
            Assert.Equal(2, GetTagCount(abacusRecord?.TagCounts, "Codemology-m.Code.CSharp.readonly"));
            Assert.Equal(2, GetTagCount(abacusRecord?.TagCounts, "Codemology-m.Code.CSharp.public"));
            Assert.Equal(1, GetTagCount(abacusRecord?.TagCounts, "Codemology-m.Code.CSharp.virtual"));
            Assert.Equal(3, GetTagCount(abacusRecord?.TagCounts, "Codemology-m.Code.CSharp.null"));
            Assert.Equal(2, GetTagCount(abacusRecord?.TagCounts, "Codemology-m.Code.CSharp.void"));
            Assert.Equal(8, GetTagCount(abacusRecord?.TagCounts, "Codemology-m.Code.CSharp.using"));
            Assert.Equal(2, GetTagCount(abacusRecord?.TagCounts, "Codemology-m.Code.CSharp.true"));
            Assert.Equal(3, GetTagCount(abacusRecord?.TagCounts, "Codemology-m.Code.CSharp.private"));
            Assert.Equal(1, GetTagCount(abacusRecord?.TagCounts, "Codemology-m.Code.CSharp.protected"));
            Assert.Equal(1, GetTagCount(abacusRecord?.TagCounts, "Codemology-m.Code.CSharp.this"));
            Assert.Equal(2, GetTagCount(abacusRecord?.TagCounts, "Codemology-m.Code.CSharp.bool"));
            Assert.Equal(1, GetTagCount(abacusRecord?.TagCounts, "Codemology-m.Code.CSharp.namespace"));
            Assert.Equal(1, GetTagCount(abacusRecord?.TagCounts, "Codemology-m.Code.CSharp.class"));
        }

        private int GetTagCount(
            List<TagCounter>? tagCounters,
            string tag)
        {
            if (tagCounters is { })
            {
                foreach (TagCounter tagCounter in tagCounters)
                {
                    if (String.Equals(tagCounter.Tag, tag))
                    {
                        return tagCounter.Count;
                    }
                }
            }

            return -1;
        }
    }
}

