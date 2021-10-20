﻿using Microsoft.ApplicationInspector.RulesEngine;
using MuddyTurnip.RulesEngine;
using System.Text;

namespace MuddyTurnip.Metrics.Engine
{
    public static class LocationIndexExtensions
    {
        public static int Compare(
            this LocationIndex a,
            LocationIndex b)
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
