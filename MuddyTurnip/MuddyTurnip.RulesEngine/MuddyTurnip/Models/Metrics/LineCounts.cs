using Microsoft.ApplicationInspector.Commands;
using Microsoft.ApplicationInspector.RulesEngine;
using MuddyTurnip.RulesEngine;
using MuddyTurnip.RulesEngine.Commands;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;

namespace MuddyTurnip.Metrics.Engine
{
    public class LineCounts
    {
        public int LineNumber { get; set; }
        public int StartIndex { get; }
        public bool OnWord { get; set; }
        public int SpacesCount { get; set; }
        public int UpperCaseCount { get; set; }
        public int LowerCaseCount { get; set; }
        public int Length { get; set; }
        public int WordCount { get; set; }
        public int UnderscoreCount { get; set; }
        public int HyphenCount { get; set; }

        public LineCounts(int startIndex)
        {
            StartIndex = startIndex;
        }
    }
}
