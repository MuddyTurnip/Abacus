﻿using Microsoft.ApplicationInspector.RulesEngine;

namespace MuddyTurnip.RulesEngine
{
    public interface IBoundaryCounter
    {
        int GetFullIndexFromCodeIndex(int codeIndex);
        Location GetLocation(int index);
    }
}