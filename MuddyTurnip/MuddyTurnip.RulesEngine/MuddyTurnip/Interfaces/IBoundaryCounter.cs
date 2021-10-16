using Microsoft.ApplicationInspector.RulesEngine;

namespace MuddyTurnip.RulesEngine
{
    public interface IBoundaryCounter
    {
        int AdjustLineNumber(int strippedIndex);
        Location GetLocation(int index);
    }
}