using System.Diagnostics;

namespace MuddyTurnip.RulesEngine
{
    [DebuggerDisplay("{CloseIndex}, {Settings.Close}")]
    public class StatementStats
    {
        public StatementBoundarySettings Settings { get; set; }
        public int CloseIndex { get; set; }
        public bool Assigned { get; set; }

        public StatementStats(
            int closeIndex,
            StatementBoundarySettings settings)
        {
            CloseIndex = closeIndex;
            Settings = settings;
        }
    }
}