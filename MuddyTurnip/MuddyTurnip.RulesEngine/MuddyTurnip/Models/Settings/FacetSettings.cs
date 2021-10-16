
using System.Collections.Generic;

namespace MuddyTurnip.RulesEngine
{
    public class FacetSettings
    {
        public string Name { get; }
        public string BlockType { get; } = string.Empty;
        public List<string> Patterns { get; } = new();

        public FacetSettings(
            string name,
            string blockType)
        {
            Name = name;
            BlockType = blockType;
        }
    }
}