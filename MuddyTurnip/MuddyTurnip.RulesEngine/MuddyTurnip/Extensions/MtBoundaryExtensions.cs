using Microsoft.ApplicationInspector.RulesEngine;
using MuddyTurnip.RulesEngine;
using MuddyTurnip.RulesEngine.Commands;
using System.Text;

namespace MuddyTurnip.Metrics.Engine
{
    public static class MtBoundaryExtensions
    {
        public static int Compare(
            this MtBoundary a,
            MtBoundary b)
        {
            if (a.Index < b.Index)
            {
                return -1;
            }

            if (a.Index == b.Index)
            {
                return 0;
            }

            return 1;
        }
    }
}
