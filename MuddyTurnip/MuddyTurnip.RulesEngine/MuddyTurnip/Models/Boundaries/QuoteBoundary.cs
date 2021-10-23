using System.Diagnostics;

namespace MuddyTurnip.RulesEngine
{
    [DebuggerDisplay("{Type}")]
    public class QuoteBoundary
    {
        public int Index { get; set; }
        public bool MultiLine { get; set; }

        public QuoteBoundary(
            int index,
            bool multiLine)
        {
            Index = index;
            MultiLine = multiLine;
        }
    }
}