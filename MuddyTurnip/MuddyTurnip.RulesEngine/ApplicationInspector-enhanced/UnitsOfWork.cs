using Newtonsoft.Json;
using System.Collections.Generic;

namespace MuddyTurnip.RulesEngine
{
    internal class UnitsOfWork
    {

        [JsonProperty(PropertyName = "nonBlockPatterns")]
        public List<Pattern> NonBlockPatterns { get; set; } = new();

        [JsonProperty(PropertyName = "enabled")]
        public bool? Enabled { get; set; }
    }
}