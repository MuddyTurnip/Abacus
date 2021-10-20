using System.Collections.Generic;

namespace MuddyTurnip.RulesEngine
{
    public class UnitOfWorkSettings
    {
        public List<PatternSettings> NonBlockPatterns { get; set; } = new();

        public UnitOfWorkSettings()
        {
        }
    }
}