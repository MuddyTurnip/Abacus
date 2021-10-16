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
                FilePathExclusions = "sample,example,test,docs,lib,.vs,.git,bin,obj,node_modules,scripts",
                ConsoleVerbosityLevel = cliOptions.ConsoleVerbosityLevel,
                //Log = cliOptions.Log,
                SingleThread = true,
                NoShowProgress = cliOptions.NoShowProgressBar,
                FileTimeOut = cliOptions.FileTimeOut,
                ProcessingTimeOut = cliOptions.ProcessingTimeOut,
                ContextLines = cliOptions.ContextLines,
                ScanUnknownTypes = false,
                TagsOnly = cliOptions.TagsOnly,
                NoFileMetadata = cliOptions.NoFileMetadata
            }); ;

            _analyzeResult = command.GetResult();
        }

        [Fact]
        public void CheckMetricCounts()
        {
            Assert.Equal(MtAnalyzeResult.ExitCode.Success, _analyzeResult?.ResultCode);
            Assert.Equal(1, _analyzeResult?.Metadata.Metrics.Count);

            MetricsRecord? record = _analyzeResult?.Metadata.Metrics[0];
            Assert.NotNull(record);

            Assert.Equal(29, GetTagCount(record?.TagCounts, "Codemology-m.Code.CSharp.string"));
            Assert.Equal(10, GetTagCount(record?.TagCounts, "Codemology-m.Code.CSharp.return"));
            Assert.Equal(9, GetTagCount(record?.TagCounts, "Codemology-m.Code.CSharp.new"));
            Assert.Equal(10, GetTagCount(record?.TagCounts, "Codemology-m.Code.CSharp.false"));
            Assert.Equal(10, GetTagCount(record?.TagCounts, "Codemology-m.Code.CSharp.internal"));
            Assert.Equal(5, GetTagCount(record?.TagCounts, "Codemology-m.Code.CSharp.if"));
            Assert.Equal(2, GetTagCount(record?.TagCounts, "Codemology-m.Code.CSharp.throw"));
            Assert.Equal(2, GetTagCount(record?.TagCounts, "Codemology-m.Code.CSharp.readonly"));
            Assert.Equal(2, GetTagCount(record?.TagCounts, "Codemology-m.Code.CSharp.public"));
            Assert.Equal(1, GetTagCount(record?.TagCounts, "Codemology-m.Code.CSharp.virtual"));
            Assert.Equal(3, GetTagCount(record?.TagCounts, "Codemology-m.Code.CSharp.null"));
            Assert.Equal(2, GetTagCount(record?.TagCounts, "Codemology-m.Code.CSharp.void"));
            Assert.Equal(8, GetTagCount(record?.TagCounts, "Codemology-m.Code.CSharp.using"));
            Assert.Equal(2, GetTagCount(record?.TagCounts, "Codemology-m.Code.CSharp.true"));
            Assert.Equal(3, GetTagCount(record?.TagCounts, "Codemology-m.Code.CSharp.private"));
            Assert.Equal(1, GetTagCount(record?.TagCounts, "Codemology-m.Code.CSharp.protected"));
            Assert.Equal(1, GetTagCount(record?.TagCounts, "Codemology-m.Code.CSharp.this"));
            Assert.Equal(2, GetTagCount(record?.TagCounts, "Codemology-m.Code.CSharp.bool"));
            Assert.Equal(1, GetTagCount(record?.TagCounts, "Codemology-m.Code.CSharp.namespace"));
            Assert.Equal(1, GetTagCount(record?.TagCounts, "Codemology-m.Code.CSharp.class"));
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

