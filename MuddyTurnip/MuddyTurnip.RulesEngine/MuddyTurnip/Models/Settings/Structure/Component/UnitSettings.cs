
using System.Collections.Generic;

namespace MuddyTurnip.RulesEngine
{
    public class UnitSettings
    {
        public string Name { get; set; } = string.Empty;
        public string BlockType { get; set; } = string.Empty;
        public List<string> Patterns { get; } = new();

        public UnitSettings(
            string name,
            string blockType)
        {
            Name = name;
            BlockType = blockType;
        }
    }
}