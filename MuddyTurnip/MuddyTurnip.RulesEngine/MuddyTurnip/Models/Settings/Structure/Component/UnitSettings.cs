
using System.Collections.Generic;

namespace MuddyTurnip.RulesEngine
{
    public class UnitSettings
    {
        public string Name { get; set; } = string.Empty;
        public string BlockType { get; } = string.Empty;
        public bool PrintMetrics { get; } = false;
        public List<string> Patterns { get; } = new();

        public UnitSettings(
            string name,
            string blockType,
            bool? printMetrics = false)
        {
            Name = name;
            BlockType = blockType;

            if (printMetrics == true)
            {
                PrintMetrics = true;
            }
        }
    }
}