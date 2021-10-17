using MuddyTurnip.RulesEngine;
using MuddyTurnip.RulesEngine.Commands;
using System.Text;

namespace MuddyTurnip.Metrics.Engine
{
    public static class MtMatchRecordExtensions
    {
        public static int Compare(
            this MtMatchRecord a,
            MtMatchRecord b)
        {
            if (a.StartIndex < b.StartIndex)
            {
                return -1;
            }

            if (a.StartIndex == b.StartIndex)
            {
                if (a.EndIndex == b.EndIndex)
                {
                    return 0;
                }

                if (a.EndIndex > b.EndIndex)
                {
                    return -1;
                }
            }

            return 1;
        }
    }
}
