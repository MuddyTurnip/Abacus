
using System.Collections.Generic;

namespace MuddyTurnip.RulesEngine
{
    public class ComponentSettings
    {
        public string ID { get; } = string.Empty;
        public string Name { get; } = string.Empty;
        public string BlockType { get; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool QualifiesName { get; set; } = false;
        public List<string> Parents { get; } = new();
        public List<UnitSettings> Units { get; } = new();
        public List<string> Patterns { get; } = new();

        public ComponentSettings(
            string id,
            string name,
            string blockType)
        {
            ID = id;
            Name = name;
            BlockType = blockType;
        }
    }
}